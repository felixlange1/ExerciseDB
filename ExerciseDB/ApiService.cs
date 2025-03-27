namespace ExerciseDB;
using System.Net.Http.Headers;
public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "5b4171f559mshe11682bfa322384p12d97ajsne55dff0edab7";
    private readonly string _baseUrl = "https://exercisedb.p.rapidapi.com/exercises";
    private readonly string _apiHost = "exercisedb.p.rapidapi.com";
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
}


