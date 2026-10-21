using CineMatch.Core.Entities;
using CineMatch.Core.Interfaces;
using CineMatch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _db;

    public ReportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<GenreReportRow>> GetGenreReportAsync(GenreReportRequest request, CancellationToken ct = default)
    {
        var rawGenres = await _db.Genres
            .Select(g => new
            {
                g.Name,
                Movies = g.Movies
                    .Where(m => m.VoteAverage >= request.MinVoteAverage)
                    .Select(m => new { m.Title, m.VoteAverage })
                    .ToList()
            })
            .ToListAsync(ct);

        var rows = rawGenres
            .Where(g => g.Movies.Count > 0)
            .Select(g => new GenreReportRow(
                GenreName: g.Name,
                MovieCount: g.Movies.Count,
                AverageVote: g.Movies.Average(m => m.VoteAverage),
                HighestVote: g.Movies.Max(m => m.VoteAverage),
                TopMovieTitle: g.Movies
                    .OrderByDescending(m => m.VoteAverage)
                    .FirstOrDefault()?.Title))
            .ToList();

        var sorted = request.SortBy switch
        {
            "MoviesCount" => rows.OrderByDescending(r => r.MovieCount),
            "HighestVote" => rows.OrderByDescending(r => r.HighestVote),
            _ => rows.OrderByDescending(r => r.AverageVote)
        };

        return sorted.Take(request.Take).ToList();
    }

    public async Task<IReadOnlyList<Movie>> GetTopMoviesByGenreAsync(
        string genreName,
        int take,
        CancellationToken ct = default)
    {
        return await _db.Movies
            .Where(m => m.Genres.Any(g => g.Name == genreName))
            .OrderByDescending(m => m.VoteAverage)
            .Take(take)
            .ToListAsync(ct);
    }
}