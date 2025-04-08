namespace ExerciseDB.Models;

public class WorkoutSet
{
    public int WorkoutId { get; set; }
    public int SetId { get; set; }
    public int SetNumber { get; set; }
    public double Weight { get; set; }
    public int Reps { get; set; }
}