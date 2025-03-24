using ExerciseDB.Models;

namespace ExerciseDB;

public interface IWorkoutRepository
{
    public Workout GetWorkout(int id);
    public IEnumerable<Workout> GetAllWorkouts();
    public void UpdateWorkout(Workout workout);
    public void CreateWorkout(Workout workout);
    public void DeleteWorkout(Workout workout);
}