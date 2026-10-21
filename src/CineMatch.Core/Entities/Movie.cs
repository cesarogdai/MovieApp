namespace CineMatch.Core.Entities;

public class Movie
{ 
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Overview { get; set; } 
    public string? PosterPath { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public double VoteAverage { get; set; }


    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}