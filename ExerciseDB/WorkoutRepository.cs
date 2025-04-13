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
        var set = _connection.Query<WorkoutSet>("SELECT * FROM workout_sets WHERE WorkoutId = @workoutId", new { workoutId = workoutId }).ToList();
      
        if (workout == null)
        {
            throw new Exception("Workout not found");
        }
        
        if (set != null)
        {
            workout.Sets = set;
        }
        
        return workout;
    }

    public IEnumerable<Workout> GetAllWorkouts()
    {
        var workouts = _connection.Query<Workout>("SELECT * FROM Workouts").ToList();

        foreach (var workout in workouts)
        {
            var sets = _connection.Query<WorkoutSet>("SELECT * FROM workout_sets WHERE WorkoutId = @workoutId", new { workoutId = workout.WorkoutId }).ToList();
            workout.Sets = sets;
        }
        
        return workouts;
    }

    public void UpdateWorkout(Workout workout)
    {
        _connection.Execute("UPDATE Workouts SET ExerciseName = @ExerciseName, WorkoutDate = @workoutDate, Notes = @Notes WHERE WorkoutId = @WorkoutId", 
            new
            {
                ExerciseName = workout.ExerciseName,
                workoutDate = workout.WorkoutDate,
                Notes = workout.Notes,
                WorkoutId = workout.WorkoutId
            });
        foreach (var set in workout.Sets)
        {
            _connection.Execute("UPDATE workout_sets SET SetNumber = @Setnumber, Weight = @Weight, Reps = @Reps WHERE SetId = @SetId AND WorkoutId = @WorkoutId",
                new
                {
                    SetNumber = set.SetNumber,
                    Weight = set.Weight,
                    Reps = set.Reps,
                    setId = set.SetId,
                    WorkoutId = workout.WorkoutId
                });
        }
    }

    public void CreateWorkout(Workout workout)
    {
        _connection.Execute(
            "INSERT INTO workouts (ExerciseName, WorkoutDate, Notes) VALUES (@exerciseName, @workoutdate, @notes);",
            new
            {
                exerciseName = workout.ExerciseName, sets = workout.Sets, workoutdate = workout.WorkoutDate, notes = workout.Notes
            });
        
        int workoutId = _connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");

        foreach (var set in workout.Sets)
        {
            _connection.Execute("INSERT INTO workout_sets (WorkoutID, SetNumber, Weight, Reps) VALUES (@WorkoutID, @SetNumber, @Weight, @Reps);",
            new
            {
                workoutID = workoutId, setNumber = set.SetNumber, weight = set.Weight, reps = set.Reps
            });
        }
    }

    public void DeleteWorkout(Workout workout)
    {
        _connection.Execute("DELETE FROM Workouts WHERE WorkoutId = @workoutId;", new { workoutId = workout.WorkoutId });
        _connection.Execute("DELETE FROM workout_sets WHERE WorkoutId = @workoutId;", new { workoutId = workout.WorkoutId });
    }
}