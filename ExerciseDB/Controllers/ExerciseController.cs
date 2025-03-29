using System.Collections;
using ExerciseDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExerciseDB.Controllers;

public class ExerciseController : Controller
{
    private readonly ApiService _apiService;

    public ExerciseController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return View();
        }
        
        var jsonResponse= await _apiService.SearchExercisesAsync(searchTerm);
        var exercises = JsonConvert.DeserializeObject<List<Exercise>>(jsonResponse);
        return View(exercises);
    }
    
}