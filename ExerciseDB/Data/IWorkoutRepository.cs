using ExerciseDB.Models;

namespace ExerciseDB;

public interface IWorkoutRepository
{
    public Workout GetWorkout(int id);
    public IEnumerable<Workout> GetAllWorkouts(string sortBy, string searchString);
    public void UpdateWorkout(Workout workout);
    public void CreateWorkout(Workout workout);
    public void DeleteWorkout(int id);
    public void DeleteSet(int id);

}