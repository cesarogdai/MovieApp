using System.Net.Http.Json;
using CineMatch.Infrastructure.Tmdb.Models;

namespace CineMatch.Infrastructure.Tmdb;

public interface ITmdbClient
{
    Task<TmdbSearchResponse> SearchMoviesAsync(string query, CancellationToken ct = default);
    Task<TmdbMovieDetails?> GetMovieDetailsAsync(int tmdbId, CancellationToken ct = default);
    Task<TmdbSearchResponse> GetPopularAsync(CancellationToken ct = default);
    Task<TmdbGenreListResponse> GetGenresAsync(CancellationToken ct = default);

    Task<TmdbSearchResponse> DiscoverMoviesAsync(
        string? genreIds = null,
        string? providerIds = null,
        string watchRegion = "MX",
        CancellationToken ct = default);

    Task<TmdbWatchProvidersResponse?> GetWatchProvidersAsync(
        int tmdbId,
        string region = "MX",
        CancellationToken ct = default);
}

public class TmdbClient : ITmdbClient
{
    private readonly HttpClient _http;

    public TmdbClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<TmdbSearchResponse> SearchMoviesAsync(string query, CancellationToken ct = default)
    {
        var url = $"search/movie?query={Uri.EscapeDataString(query)}&include_adult=false";
        return await _http.GetFromJsonAsync<TmdbSearchResponse>(url, ct)
               ?? new TmdbSearchResponse();
    }

    public async Task<TmdbMovieDetails?> GetMovieDetailsAsync(int tmdbId, CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<TmdbMovieDetails>($"movie/{tmdbId}", ct);
    }

    public async Task<TmdbSearchResponse> GetPopularAsync(CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<TmdbSearchResponse>("movie/popular", ct)
               ?? new TmdbSearchResponse();
    }

    public async Task<TmdbGenreListResponse> GetGenresAsync(CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<TmdbGenreListResponse>("genre/movie/list", ct)
               ?? new TmdbGenreListResponse();
    }

    public async Task<TmdbSearchResponse> DiscoverMoviesAsync(
        string? genreIds = null,
        string? providerIds = null,
        string watchRegion = "MX",
        CancellationToken ct = default)
    {
        var query = new List<string> { "sort_by=popularity.desc" };

        if (!string.IsNullOrWhiteSpace(genreIds))
        {
            query.Add($"with_genres={Uri.EscapeDataString(genreIds)}");
        }

        if (!string.IsNullOrWhiteSpace(providerIds))
        {
            query.Add($"with_watch_providers={Uri.EscapeDataString(providerIds)}");
            query.Add($"watch_region={Uri.EscapeDataString(watchRegion)}");
        }

        var url = $"discover/movie?{string.Join('&', query)}";
        return await _http.GetFromJsonAsync<TmdbSearchResponse>(url, ct)
               ?? new TmdbSearchResponse();
    }

    public async Task<TmdbWatchProvidersResponse?> GetWatchProvidersAsync(
        int tmdbId,
        string region = "MX",
        CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<TmdbWatchProvidersResponse>(
            $"movie/{tmdbId}/watch/providers", ct);
    }
}