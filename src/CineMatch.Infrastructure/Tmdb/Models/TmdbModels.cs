using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;

namespace CineMatch.Infrastructure.Tmdb.Models;

public class TmdbSearchResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }

    [JsonPropertyName("results")]
    public List<TmdbMovieSummary> Results { get; set; } = new();
}

public class TmdbMovieSummary
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("genre_ids")]
    public List<int> GenreIds { get; set; } = new();

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }
}

public class TmdbMovieDetails : TmdbMovieSummary
{
    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenre> Genres { get; set; } = new();
}

public class TmdbGenre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class TmdbGenreListResponse
{
    [JsonPropertyName("genres")]
    public List<TmdbGenre> Genres { get; set; } = new();
}

public class TmdbWatchProvidersResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("results")]
    public Dictionary<string, TmdbWatchProviderRegion> Results { get; set; } = new();
}

public class TmdbWatchProviderRegion
{
    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("flatrate")]
    public List<TmdbProviderEntry>? Flatrate { get; set; }

    [JsonPropertyName("rent")]
    public List<TmdbProviderEntry>? Rent { get; set; }

    [JsonPropertyName("buy")]
    public List<TmdbProviderEntry>? Buy { get; set; }
}

public class TmdbProviderEntry
{
    [JsonPropertyName("provider_id")]
    public int ProviderId { get; set; }

    [JsonPropertyName("provider_name")]
    public string ProviderName { get; set; } = string.Empty;

    [JsonPropertyName("logo_path")]
    public string LogoPath { get; set; } = string.Empty;

    [JsonPropertyName("display_priority")]
    public int DisplayPriority { get; set; }
}