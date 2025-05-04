using System.Collections;
using ExerciseDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace ExerciseDB.Controllers;

public class ExerciseController : Controller
{
    private readonly ApiService _apiService;

    public ExerciseController(ApiService apiService)
    {
        _apiService = apiService;
    }


    // Handles exercise search and returns formatted results to the view:
    public async Task<IActionResult> Index(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return View();
        }
        var textInfo = new CultureInfo("en-US").TextInfo; // Used to capitalize each word in exercise names
        
        var jsonResponse= await _apiService.SearchExercisesAsync(searchTerm);
        Console.WriteLine($"API Response: {jsonResponse}");
        
        var exercises = JsonConvert.DeserializeObject<List<Exercise>>(jsonResponse);
        
        foreach (var exercise in exercises)
        {
            exercise.Name = textInfo.ToTitleCase(exercise.Name); // Used to capitalize each word in the exercise name
        }
        
        return View(exercises);
    }

    // Method called via JavaScript (search.js) for dynamic live search:
    public async Task<IActionResult> LiveSearch(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return Json(new List<Exercise>()); // returns empty Json to JS
        }
        var textInfo = new CultureInfo("en-US").TextInfo;
        
        var jsonResponse = await _apiService.SearchExercisesAsync(searchTerm);
        var exercises = JsonConvert.DeserializeObject<List<Exercise>>(jsonResponse);
        
        foreach (var exercise in exercises)
        {
            exercise.Name = textInfo.ToTitleCase(exercise.Name);
        }
        
        return Json(exercises);
    }
    
}