namespace ExerciseDB.Models;

public class Workout
{
    public int Id { get; set; }
    public string ExerciseName { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
    public DateTime WorkoutDate { get; set; }
    public string Notes { get; set; }
}