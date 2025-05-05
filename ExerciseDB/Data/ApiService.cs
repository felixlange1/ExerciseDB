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


    // Method to call external API to get exercise data:
    public async Task<string> SearchExercisesAsync(string searchTerm)
    {
            var requestUri = $"{_baseUrl}/name/{searchTerm}?offset=0&limit=10";
            return await _httpClient.GetStringAsync(requestUri);
    }
    
}


