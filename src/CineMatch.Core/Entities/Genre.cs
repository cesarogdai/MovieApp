namespace CineMatch.Core.Entities;

public class Genre
{
    public int Id { get; set; }
    public int TmdbGenreId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}