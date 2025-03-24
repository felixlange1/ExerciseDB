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
    
    public Workout GetWorkout(int id)
    {
        return _connection.QuerySingle<Workout>("SELECT * FROM Workouts WHERE Id = @id", new { id = id });
    }

    public IEnumerable<Workout> GetAllWorkouts()
    {
        return _connection.Query<Workout>("SELECT * FROM Workouts");
    }

    public void UpdateWorkout(Workout workout)
    {
        _connection.Execute("UPDATE Workouts SET ExerciseName = @ExerciseName, Sets = @Sets, Reps = @Reps, WorkoutDate = @workoutDate, Notes = @Notes WHERE Id = @Id", new { ExerciseName = workout.ExerciseName, Sets = workout.Sets, Reps = workout.Reps, workoutDate = workout.Date, Notes = workout.Notes});
    }

    public void CreateWorkout(Workout workout)
    {
        _connection.Execute(
            "INSERT INTO workouts (Name, Sets, Reps, WorkoutDate, Notes) VALUES (@name, @sets, @reps, @workoutdate, @notes);",
            new
            {
                name = workout.ExerciseName, sets = workout.Sets, reps = workout.Reps, workoutdate = workout.Date, notes = workout.Notes
            });
    }

    public void DeleteWorkout(Workout workout)
    {
        _connection.Execute("DELETE FROM Workouts WHERE ID = @id;", new { id = workout.Id });
    }
}