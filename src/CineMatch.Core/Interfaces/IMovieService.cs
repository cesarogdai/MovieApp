using CineMatch.Core.Entities;

namespace CineMatch.Core.Interfaces;

public interface IMovieService
{
    Task<IReadOnlyList<Movie>> GetFeaturedMoviesAsync(CancellationToken cancellationToken = default);
    Task<Movie?> GetByTmdbIdAsync(int tmdbId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Movie>> SearchAsync(string query, CancellationToken cancellation = default);

    Task<IReadOnlyList<Movie>> DiscoverAsync(string? genreIds, string? providerIds, CancellationToken ct = default);

    Task<IReadOnlyList<WatchProvider>> GetWatchProvidersAsync(int tmdbId, CancellationToken ct = default);
}