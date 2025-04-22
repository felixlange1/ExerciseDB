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
        Console.WriteLine($"API Response: {jsonResponse}");
        
        var exercises = JsonConvert.DeserializeObject<List<Exercise>>(jsonResponse);
        return View(exercises);
    }

    public async Task<IActionResult> LiveSearch(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return Json(new List<Exercise>());
        }
        var jsonResponse = await _apiService.SearchExercisesAsync(searchTerm);
        var exercises = JsonConvert.DeserializeObject<List<Exercise>>(jsonResponse);
        return Json(exercises);
    }
    
    // [HttpPost]
    // public async Task<IActionResult> ViewExercise()
    // {
    //     using (var reader = new StreamReader(Request.Body))
    //     {
    //         var response = await reader.ReadToEndAsync();
    //         var exercise = JsonConvert.DeserializeObject<Exercise>(response);
    //         if (exercise == null)
    //         {
    //             return BadRequest("Exercise is null");
    //         }
    //         return View(exercise);
    //     }
    //
    // }
    
}