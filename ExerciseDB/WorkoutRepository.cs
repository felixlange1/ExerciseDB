using System.Data;
using System.Text;
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
        var workout = _connection.QuerySingleOrDefault<Workout>("SELECT * FROM Workouts WHERE WorkoutId = @workoutId", new { workoutId = id });
        var set = _connection.Query<WorkoutSet>("SELECT * FROM workout_sets WHERE WorkoutId = @workoutId", new { workoutId = id }).ToList();
      
        if (workout == null)
        {
            throw new Exception("Workout not found");
        }
        
            workout.Sets = set;
        
        
        return workout;
    }

    
    
    // public IEnumerable<Workout> SearchWorkout(string searchString, string sortBy)
    // {
    //     string query = "SELECT * FROM Workouts WHERE(ExerciseName) LIKE %@searchString%";
    //
    //
    //     
    //     switch (sortBy)
    //     {
    //         case "Name":
    //             query += " ORDER BY ExerciseName, WorkoutDate";
    //             break;
    //         case "NameDesc":
    //             query += " ORDER BY ExerciseName DESC, WorkoutDate";
    //             break;
    //         case "Date":
    //             query += " ORDER BY WorkoutDate";
    //             break;
    //         case "DateDesc":
    //             query += " ORDER BY WorkoutDate DESC";
    //             break;
    //         default:
    //             query += " ORDER BY WorkoutDate DESC";
    //             break;
    //     }
    //
    //     var workouts = _connection.Query<Workout>(query, new { searchString = $"%{searchString.ToLower()}%" }).ToList();
    //     
    //     foreach (var workout in workouts)
    //     {
    //         var sets = _connection.Query<WorkoutSet>("SELECT * FROM workout_sets WHERE WorkoutId = @workoutId", new { workoutId = workout.WorkoutId }).ToList();
    //         workout.Sets = sets;
    //     }
    //     
    //     return workouts;
    // }
    //
    
    
    
    
    
    public IEnumerable<Workout> GetAllWorkouts(string sortBy, string searchString)
    {
        
        var query = new StringBuilder("SELECT * FROM Workouts");
        
        if (searchString != null)
        {
            query.Append(" WHERE (ExerciseName) LIKE @searchString");
        }

        switch (sortBy)
        {
            case "Name":
                query.Append(" ORDER BY ExerciseName, WorkoutDate");
                break;
            case "NameDesc":
                query.Append(" ORDER BY ExerciseName DESC, WorkoutDate");
                break;
            case "Date":
                query.Append(" ORDER BY WorkoutDate");
                break;
            case "DateDesc":
                query.Append(" ORDER BY WorkoutDate DESC");
                break;
            default:
                query.Append(" ORDER BY WorkoutDate DESC");
                break;
        }

        IEnumerable<Workout> workouts;
        
        if (searchString != null)
        {
            workouts = _connection.Query<Workout>(query.ToString(), new { searchString = $"%{searchString.ToLower()}%" }).ToList();
        }
        else
        {
            workouts = _connection.Query<Workout>(query.ToString()).ToList();
        }
        
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
                WorkoutDate = workout.WorkoutDate,
                Notes = workout.Notes,
                WorkoutId = workout.WorkoutId
            });
        foreach (var set in workout.Sets)
        {
            Console.WriteLine($"Set ID: {set.SetId}, Set #: {set.SetNumber}, Reps: {set.Reps}, Weight: {set.Weight}");

            if (set.SetId > 0)
            {
                _connection.Execute(
                    "UPDATE workout_sets SET SetNumber = @SetNumber, Weight = @Weight, Reps = @Reps WHERE SetId = @SetId AND WorkoutId = @WorkoutId",
                    new
                    {
                        SetNumber = set.SetNumber,
                        Weight = set.Weight,
                        Reps = set.Reps,
                        SetId = set.SetId,
                        WorkoutId = workout.WorkoutId
                    });
            }
            else
            {
                _connection.Execute(
                    "INSERT INTO workout_sets (WorkoutID, SetNumber, Weight, Reps) VALUES (@WorkoutID, @SetNumber, @Weight, @Reps);",
                    new
                    {
                        workoutID = workout.WorkoutId,
                        setNumber = set.SetNumber,
                        weight = set.Weight,
                        reps = set.Reps
                    });
            }
            
        }
    }

    public void CreateWorkout(Workout workout)
    {
        workout.Sets = workout.Sets.Where(set => set.Reps > 0 && set.Weight > 0).ToList();
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

    public void DeleteWorkout(int id)
    {
        _connection.Execute("DELETE FROM workout_sets WHERE WorkoutId = @workoutId;", new { workoutId = id });
        _connection.Execute("DELETE FROM Workouts WHERE WorkoutId = @workoutId;", new { workoutId = id });
    }

    public void DeleteSet(int id)
    {
        _connection.Execute("DELETE FROM workout_sets WHERE SetId = @setId;", new { setId = id });
    }

}