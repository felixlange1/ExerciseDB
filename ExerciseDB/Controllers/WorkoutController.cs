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

    public IActionResult Index(string sortBy, string searchString)
    {
        var workouts = repo.GetAllWorkouts(sortBy, searchString);
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

    public IActionResult UpdateWorkoutToDatabase(Workout workout, List<int> DeletedSetIds)
    {
        Console.WriteLine("WorkoutId received from form: " + workout.WorkoutId);
        if (workout.Sets == null)
        {
            Console.WriteLine("No sets received.");
        }
        else
        {
            Console.WriteLine("Number of sets: " + workout.Sets.Count);
        }

        foreach (var id in DeletedSetIds)
        {
            repo.DeleteSet(id);
        }
   //     workout.NumberOfSets = workout.Sets.Count;
        repo.UpdateWorkout(workout);
        return RedirectToAction("ViewWorkout", new { id = workout.WorkoutId });
    }

    public IActionResult CreateWorkout(string exerciseName)
    {
        
        ViewBag.ExerciseName = exerciseName;
        var workout = new Workout();

        return View(workout);
    }
    public IActionResult CreateWorkoutToDataBase(Workout workoutToCreate)
    {
   //     workoutToCreate.NumberOfSets = workoutToCreate.Sets.Count;
        repo.CreateWorkout(workoutToCreate);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteWorkout(int id)
    {
        repo.DeleteWorkout(id);
        return RedirectToAction("Index");
    }

    // public IActionResult SearchWorkout(string searchString, string sortBy)
    // {
    //     ViewBag.searchString = searchString;
    //     
    //     var workouts = repo.GetAllWorkouts(sortBy).Where(w => w.ExerciseName.Contains(searchString));
    //     return View("Index", workouts);
    // }
    
}