using ExerciseDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseDB.Controllers;

public class WorkoutController : Controller
{
    private readonly IWorkoutRepository repo;
    private readonly HttpClient _client;
    private readonly string _apiKey;
    
    public WorkoutController(IWorkoutRepository repo)
    {
        this.repo = repo;
    }

    public IActionResult Index()
    {
        var workouts = repo.GetAllWorkouts();
        return View(workouts);
    }
    
    
    public IActionResult ViewWorkout(int id)
    {
        var workout = repo.GetWorkout(id);
        return View(workout);
    }

    public IActionResult UpdateWorkout(int id)
    {
        Workout workout = repo.GetWorkout(id);
        if (workout == null)
        {
            return NotFound();
        }
        return View(workout);
    }

    public IActionResult UpdateWorkoutToDatabase(Workout workout)
    {
        workout.NumberOfSets = workout.Sets.Count;
        repo.UpdateWorkout(workout);
        return RedirectToAction("ViewWorkout", new { workoutId = workout.WorkoutId });
    }

    public IActionResult CreateWorkout(string exerciseName)
    {
        
        ViewBag.ExerciseName = exerciseName;
        return View();
    }
    public IActionResult CreateWorkoutToDataBase(Workout workoutToCreate)
    {
        workoutToCreate.NumberOfSets = workoutToCreate.Sets.Count;
        repo.CreateWorkout(workoutToCreate);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteWorkout(Workout workoutToDelete)
    {
        repo.DeleteWorkout(workoutToDelete);
        return RedirectToAction("Index");
    }
}