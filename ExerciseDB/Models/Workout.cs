namespace ExerciseDB.Models;

public class Workout
{
    public int WorkoutId { get; set; } 
    public string ExerciseName { get; set; }
  //  public int NumberOfSets { get; set; } = 3; // view model instead?
    public List<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
    public DateTime WorkoutDate { get; set; }
    public string Notes { get; set; }
}