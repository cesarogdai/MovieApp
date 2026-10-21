using CineMatch.Core.Entities;

namespace CineMatch.Core.Interfaces;

public record GenreReportRequest(
    int MinVoteAverage = 0,
    int Take = 20,
    string SortBy = "AverageVote");

public interface IReportService
{
    Task<IReadOnlyList<GenreReportRow>> GetGenreReportAsync(
        GenreReportRequest request,
        CancellationToken ct = default);

    Task<IReadOnlyList<Movie>> GetTopMoviesByGenreAsync(
        string genreName,
        int take,
        CancellationToken ct = default);
}