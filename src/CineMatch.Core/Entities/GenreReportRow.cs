namespace CineMatch.Core.Entities;

public record GenreReportRow(
    string GenreName,
    int MovieCount,
    double AverageVote,
    double HighestVote,
    string? TopMovieTitle);