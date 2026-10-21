using CineMatch.Core.Entities;
using CineMatch.Core.Interfaces;
using CineMatch.Infrastructure.Data;
using CineMatch.Infrastructure.Tmdb;
using Microsoft.EntityFrameworkCore;
namespace CineMatch.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly AppDbContext _db;
    private readonly ITmdbClient _tmdb;

    public MovieService(AppDbContext db, ITmdbClient tmdb)
    {
        _db = db;
        _tmdb = tmdb;
    }

    public async Task<IReadOnlyList<Movie>> GetFeaturedMoviesAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Movies
            .Include(m => m.Genres)
            .OrderByDescending(m => m.VoteAverage)
            .Take(20)
            .ToListAsync(cancellationToken);
    }

    public async Task<Movie?> GetByTmdbIdAsync(int tmdbId, CancellationToken cancellationToken = default)
    {
        var cached = await _db.Movies
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.TmdbId == tmdbId, cancellationToken);

        if (cached is not null)
        {
            return cached;
        }

        var details = await _tmdb.GetMovieDetailsAsync(tmdbId, cancellationToken);
        if (details is null)
        {
            return null;
        }

        return await UpsertFromTmdbAsync(details, cancellationToken);
    }

    public async Task<IReadOnlyList<Movie>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Array.Empty<Movie>();
        }

        var tmdbResults = await _tmdb.SearchMoviesAsync(query, cancellationToken);

        var results = new List<Movie>();
        foreach (var hit in tmdbResults.Results)
        {
            var cached = await UpsertFromTmdbAsync(hit, cancellationToken);
            results.Add(cached);
        }

        return results;
    }

    private async Task<Movie> UpsertFromTmdbAsync(
        Tmdb.Models.TmdbMovieSummary hit,
        CancellationToken cancellationToken)
    {
        var existing = await _db.Movies
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.TmdbId == hit.Id, cancellationToken);

        if (existing is not null)
        {
            return existing;
        }

        var movie = new Movie
        {
            TmdbId = hit.Id,
            Title = hit.Title,
            Overview = hit.Overview,
            PosterPath = hit.PosterPath,
            ReleaseDate = ParseReleaseDate(hit.ReleaseDate),
            VoteAverage = hit.VoteAverage,
        };

        foreach (var genreId in hit.GenreIds)
        {
            var genre = await _db.Genres
                .FirstOrDefaultAsync(g => g.TmdbGenreId == genreId, cancellationToken);

            if (genre is null)
            {
                genre = new Genre { TmdbGenreId = genreId, Name = $"Genre {genreId}" };
                _db.Genres.Add(genre);
            }

            movie.Genres.Add(genre);
        }

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync(cancellationToken);

        return movie;
    }

    private static DateOnly? ParseReleaseDate(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        return DateOnly.TryParse(raw, out var date) ? date : null;
    }

    public async Task<IReadOnlyList<Movie>> DiscoverAsync(string? genreIds, string? providerIds, CancellationToken ct = default)
    {
        var tmdbResults = await _tmdb.DiscoverMoviesAsync(genreIds, providerIds, "MX", ct);

        var results = new List<Movie>();
        foreach (var hit in tmdbResults.Results)
        {
            var cached = await UpsertFromTmdbAsync(hit, ct);
            results.Add(cached);
        }

        return results;
    }

    public async Task<IReadOnlyList<WatchProvider>> GetWatchProvidersAsync(int tmdbId, CancellationToken ct = default)
    {
        var response = await _tmdb.GetWatchProvidersAsync(tmdbId, "MX", ct);
        if (response is null || !response.Results.TryGetValue("MX", out var mx))
        {
            return Array.Empty<WatchProvider>();
        }

        var providers = new List<WatchProvider>();

        if (mx.Flatrate is not null)
        {
            foreach (var p in mx.Flatrate)
            {
                providers.Add(new WatchProvider(p.ProviderName, p.LogoPath, "flatrate"));
            }
        }

        if (mx.Rent is not null)
        {
            foreach (var p in mx.Rent)
            {
                providers.Add(new WatchProvider(p.ProviderName, p.LogoPath, "rent"));
            }
        }

        if (mx.Buy is not null)
        {
            foreach (var p in mx.Buy)
            {
                providers.Add(new Core.Entities.WatchProvider(p.ProviderName, p.LogoPath, "buy"));
            }
        }

        return providers;
    }
}