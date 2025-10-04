using ExerciseDB.Models;

namespace ExerciseDB;

public interface IWorkoutRepository
{
    public Workout GetWorkout(int id, string UserId);
    public IEnumerable<Workout> GetAllWorkouts(string sortBy, string searchString, string userId);
    public void UpdateWorkout(Workout workout, string UserId);
    public void CreateWorkout(Workout workout, string UserId);
    public void DeleteWorkout(int id, string UserId);
    public void DeleteSet(int id, string UserId);

}