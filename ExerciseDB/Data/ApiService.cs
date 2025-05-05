using System.Text.Json;
using ExerciseDB.Models;

namespace ExerciseDB;
using System.Net.Http.Headers;

// Service responsible for communicating with the external exercise API via RapidAPI.
public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _apiHost;
  
    
    // Initializes HttpClient with API key, host, and base URL from configuration (appsettings.json).
    public ApiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        
        _apiKey = config["RapidAPI:Key"];
        _apiHost = config["RapidAPI:Host"];
        _baseUrl = config["RapidAPI:BaseUrl"];
        
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", _apiHost);
    }


    // Sends a GET request to the external exercise API to search for exercises by name.
    // // Returns the raw JSON response as a string.
    public async Task<string> SearchExercisesAsync(string searchTerm)
    {
            var requestUri = $"{_baseUrl}/name/{searchTerm}?offset=0&limit=10";
            return await _httpClient.GetStringAsync(requestUri);
    }
    
}


