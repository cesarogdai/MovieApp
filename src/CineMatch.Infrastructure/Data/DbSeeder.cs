using CineMatch.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Movies.AnyAsync())
        {
            return; // already seeded
        }

        var action = new Genre { TmdbGenreId = 28, Name = "Action" };
        var scifi = new Genre { TmdbGenreId = 878, Name = "Science Fiction" };
        var drama = new Genre { TmdbGenreId = 18, Name = "Drama" };
        var thrill = new Genre { TmdbGenreId = 53, Name = "Thriller" };
        var adv = new Genre { TmdbGenreId = 12, Name = "Adventure" };

        db.Genres.AddRange(action, scifi, drama, thrill, adv);

        db.Movies.AddRange(
            new Movie
            {
                TmdbId = 603,
                Title = "The Matrix",
                Overview = "A hacker learns the truth about his reality and his role in the war against its controllers.",
                ReleaseDate = new DateOnly(1999, 3, 31),
                VoteAverage = 8.2,
                PosterPath = "/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                Genres = new List<Genre> { action, scifi }
            },
            new Movie
            {
                TmdbId = 27205,
                Title = "Inception",
                Overview = "A thief who steals corporate secrets through dream-sharing technology is given an inverse task.",
                ReleaseDate = new DateOnly(2010, 7, 16),
                VoteAverage = 8.4,
                PosterPath = "/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg",
                Genres = new List<Genre> { action, scifi, thrill }
            },
            new Movie
            {
                TmdbId = 157336,
                Title = "Interstellar",
                Overview = "A team travels through a wormhole in space in an attempt to ensure humanity's survival.",
                ReleaseDate = new DateOnly(2014, 11, 7),
                VoteAverage = 8.4,
                PosterPath = "/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                Genres = new List<Genre> { adv, drama, scifi }
            },
            new Movie
            {
                TmdbId = 155,
                Title = "The Dark Knight",
                Overview = "Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                ReleaseDate = new DateOnly(2008, 7, 18),
                VoteAverage = 8.5,
                PosterPath = "/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                Genres = new List<Genre> { action, drama, thrill }
            },
            new Movie
            {
                TmdbId = 550,
                Title = "Fight Club",
                Overview = "An insomniac office worker and a devil-may-care soap maker form an underground fight club.",
                ReleaseDate = new DateOnly(1999, 10, 15),
                VoteAverage = 8.4,
                PosterPath = "/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg",
                Genres = new List<Genre> { drama, thrill }
            }
        );

        await db.SaveChangesAsync();
    }
}