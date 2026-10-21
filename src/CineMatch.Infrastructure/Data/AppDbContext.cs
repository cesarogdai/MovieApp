using CineMatch.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace CineMatch.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasIndex(m => m.TmdbId).IsUnique();
            entity.Property(m => m.Title).HasMaxLength(500).IsRequired();
            entity.Property(m => m.PosterPath).HasMaxLength(500);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasIndex(g => g.TmdbGenreId).IsUnique();
            entity.Property(g => g.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MoviesGenres"));
    }
}