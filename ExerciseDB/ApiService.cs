using System.Text.Json;
using ExerciseDB.Models;

namespace ExerciseDB;
using System.Net.Http.Headers;
public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _apiHost;
    public ApiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        
        _apiKey = config["RapidAPI:Key"];
        _apiHost = config["RapidAPI:Host"];
        _baseUrl = config["RapidAPI:BaseUrl"];
        
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", _apiHost);
    }

    public async Task<string> GetExercisesAsync(int limit = 10, int offset = 0)
    {
        var requestUri = $"{_baseUrl}?limit={limit}&offset={offset}";
        var response = await _httpClient.GetAsync(requestUri);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<string> SearchExercisesAsync(string searchTerm)
    {
            var requestUri = $"{_baseUrl}/name/{searchTerm}?offset=0&limit=10";
            return await _httpClient.GetStringAsync(requestUri);
    }
    
}


