using System.Collections;
using ExerciseDB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace ExerciseDB.Controllers;

// Controller responsible for handling exercise search via external API and returning results to views or JavaScript.
public class ExerciseController : Controller
{
    private readonly ApiService _apiService;

    public ExerciseController(ApiService apiService)
    {
        _apiService = apiService;
    }


    // Sends search requests to an external exercise API, formats results, and returns them to the view.
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

    
    // Called via JavaScript for live search. Sends a request to an external API and returns matching exercises as JSON.
    public async Task<IActionResult> LiveSearch(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return Json(new List<Exercise>()); // returns empty Json to JS
        }
        var textInfo = new CultureInfo("en-US").TextInfo;
        
        var jsonResponse = await _apiService.SearchExercisesAsync(searchTerm);
        Console.WriteLine($"API Live Search Response: {jsonResponse}");
        
        var exercises = JsonConvert.DeserializeObject<List<Exercise>>(jsonResponse);
        
        foreach (var exercise in exercises)
        {
            exercise.Name = textInfo.ToTitleCase(exercise.Name);
        }
        
        return Json(exercises);
    }
    
}