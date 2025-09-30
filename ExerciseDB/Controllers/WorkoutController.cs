using System.Security.Claims;
using ExerciseDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExerciseDB.Controllers;

// Controller responsible for handling workout-related views and actions, including create, read, update, and delete operations.
[Authorize]
public class WorkoutController : Controller
{
    private readonly IWorkoutRepository repo;
    private readonly HttpClient _client;
    private readonly string _apiKey;
    private readonly UserManager<IdentityUser> _userManager;
    
    public WorkoutController(IWorkoutRepository repo)
    {
        this.repo = repo;
    }

    
    // Displays a list of all workouts, optionally sorted and/or filtered by a search string.
    public IActionResult Index(string sortBy, string searchString)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var workouts = repo.GetAllWorkouts(sortBy, searchString, userId);
        return View(workouts);
    }
    
    
    // Displays details for a single workout, including its sets.
    public IActionResult ViewWorkout(int id)
    {
        var workout = repo.GetWorkout(id);
        return View(workout);
    }

    
    // Loads the update form for a specific workout by ID.
    public IActionResult UpdateWorkout(int id)
    {
        Workout workout = repo.GetWorkout(id);
        if (workout == null)
        {
            return NotFound();
        }
        return View(workout);
    }

    // Handles form submission for workout updates, including updating and/or deletion of sets.
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

    // Loads the form for creating a new workout, optionally pre-filling the exercise name.
    public IActionResult CreateWorkout(string exerciseName)
    {
        // Passes selected exercise name to the view using ViewBag, for pre-filling the exercise name field:
        if (exerciseName != null)
        {
            ViewBag.ExerciseName = exerciseName;
        }

        var workout = new Workout();

        return View(workout);
    }
    
    // Handles form submission to create a new workout and its sets in the database.
    public IActionResult CreateWorkoutToDataBase(Workout workoutToCreate)
    {
   
        repo.CreateWorkout(workoutToCreate);
        return RedirectToAction("Index");
    }

    // Deletes a workout and redirects to the index view.
    public IActionResult DeleteWorkout(int id)
    {
        repo.DeleteWorkout(id);
        return RedirectToAction("Index");
    }
    
}