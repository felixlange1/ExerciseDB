using System.Data;
using ExerciseDB.Models;
using Dapper;

namespace ExerciseDB;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly IDbConnection _connection;

    public WorkoutRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    
    public Workout GetWorkout(int workoutId)
    {
        var workout = _connection.QuerySingleOrDefault<Workout>("SELECT * FROM Workouts WHERE WorkoutId = @workoutId", new { workoutId = workoutId });

        if (workout == null)
        {
            throw new Exception("Workout not found");
        }
        Console.WriteLine($"Retrieved Date: {workout?.WorkoutDate}");
        return workout;
    }

    public IEnumerable<Workout> GetAllWorkouts()
    {
        return _connection.Query<Workout>("SELECT * FROM Workouts");
    }

    public void UpdateWorkout(Workout workout)
    {
        _connection.Execute("UPDATE Workouts SET ExerciseName = @ExerciseName, Sets = @Sets, Reps = @Reps, WorkoutDate = @workoutDate, Notes = @Notes WHERE WorkoutId = @WorkoutId", new { ExerciseName = workout.ExerciseName, Sets = workout.Sets, Reps = workout.Reps, workoutDate = workout.WorkoutDate, Notes = workout.Notes, WorkoutId = workout.WorkoutId });
    }

    public void CreateWorkout(Workout workout)
    {
        _connection.Execute(
            "INSERT INTO workouts (ExerciseName, Sets, Reps, WorkoutDate, Notes) VALUES (@exerciseName, @sets, @reps, @workoutdate, @notes);",
            new
            {
                exerciseName = workout.ExerciseName, sets = workout.Sets, reps = workout.Reps, workoutdate = workout.WorkoutDate, notes = workout.Notes
            });
    }

    public void DeleteWorkout(Workout workout)
    {
        _connection.Execute("DELETE FROM Workouts WHERE WorkoutId = @workoutId;", new { workoutId = workout.WorkoutId });
    }
}