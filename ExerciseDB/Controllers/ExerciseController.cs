using Microsoft.AspNetCore.Mvc;

namespace ExerciseDB.Controllers;

public class ExerciseController : Controller
{
    private readonly ApiService _apiService;

    public ExerciseController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var exercises = await _apiService.GetExercisesAsync();
        return View(exercises);
    }
    
}