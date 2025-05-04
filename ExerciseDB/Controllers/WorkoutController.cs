using ExerciseDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseDB.Controllers;

// Controller for managing workouts: creating, viewing, updating, and deleting
public class WorkoutController : Controller
{
    private readonly IWorkoutRepository repo;
    private readonly HttpClient _client;
    private readonly string _apiKey;
    
    public WorkoutController(IWorkoutRepository repo)
    {
        this.repo = repo;
    }

    
    // Index method (for all workouts) accepts a sortBy and searchString to automatically sort results:
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

    // Handles form submission for workout updates, including deletion of sets.
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

        // Delete sets that the user marked for removal:
        if (DeletedSetIds != null && DeletedSetIds.Any())
        {
            foreach (var id in DeletedSetIds)
            {
                repo.DeleteSet(id);
            }
        }

        repo.UpdateWorkout(workout);
        return RedirectToAction("ViewWorkout", new { id = workout.WorkoutId });
    }

    public IActionResult CreateWorkout(string exerciseName)
    {
        // Passes selected exercise name to the view using ViewBag, for pre-filling the exercise name field:
        ViewBag.ExerciseName = exerciseName; 
        
        var workout = new Workout();

        return View(workout);
    }
    public IActionResult CreateWorkoutToDataBase(Workout workoutToCreate)
    {
   
        repo.CreateWorkout(workoutToCreate);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteWorkout(int id)
    {
        repo.DeleteWorkout(id);
        return RedirectToAction("Index");
    }
    
}