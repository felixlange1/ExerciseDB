namespace ExerciseDB.Models;

public class Workout
{
    public int WorkoutId { get; set; } // need to change this in repo and controller
    public string ExerciseName { get; set; }
    public int NumberOfSets { get; set; } = 3; // for view only, may create View Model later
    public List<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
    public DateTime WorkoutDate { get; set; }
    public string Notes { get; set; }
}