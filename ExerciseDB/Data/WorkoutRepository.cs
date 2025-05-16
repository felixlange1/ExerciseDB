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

    
    // Retrieves a workout by ID, including its associated sets. Uses SQL stored procedures.
    public Workout GetWorkout(int id)
    {
        using var workoutsWithSets = _connection.QueryMultiple(
            "GetWorkoutWithSets", 
            new { p_workoutId = id },
            commandType: CommandType.StoredProcedure);
        
        // WITHOUT STORED PROCEDURES:
        // var workout = _connection.QuerySingleOrDefault<Workout>("SELECT * FROM Workouts WHERE WorkoutId = @workoutId", new { workoutId = id });
        // var set = _connection.Query<WorkoutSet>("SELECT * FROM workout_sets WHERE WorkoutId = @workoutId", new { workoutId = id }).ToList();
        
        var workout = workoutsWithSets.ReadSingleOrDefault<Workout>();
        
        if (workout == null)
        {
            throw new Exception("Workout not found");
        }
        
            workout.Sets = workoutsWithSets.Read<WorkoutSet>().ToList();
            
        return workout;
    }

    
    // Retrieves all workouts, optionally filtered by a search string and sorted by the specified criteria.
    public IEnumerable<Workout> GetAllWorkouts(string sortBy, string searchString)
    {
        // Standard Query to get all workouts:
        var query = new StringBuilder("SELECT * FROM Workouts");
        
        // Adds searchString if a searchString exists:
        if (searchString != null)
        {
            query.Append(" WHERE (ExerciseName) LIKE @searchString");
        }

        // Adds how results should be sorted:
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
        
        // Calls database two different ways, depending on whether there's a searchString or not:
        if (searchString != null)
        {
            workouts = _connection.Query<Workout>(query.ToString(), new { searchString = $"%{searchString.ToLower()}%" }).ToList();
        }
        else
        {
            workouts = _connection.Query<Workout>(query.ToString()).ToList();
        }
        
        // Gets the sets from the database (using a stored procedure):
        foreach (var workout in workouts)
        {
            var sets = _connection.Query<WorkoutSet>(
                "GetWorkoutSets",
                new { p_workoutId = workout.WorkoutId },
                commandType: CommandType.StoredProcedure).ToList();
            
            // GETTING SETS WITHOUT USING A STORED PROCEDURE:
            // var sets = _connection.Query<WorkoutSet>("SELECT * FROM workout_sets WHERE WorkoutId = @workoutId", new { workoutId = workout.WorkoutId }).ToList();
            workout.Sets = sets;
        }

        return workouts;
    }

    // Updates an existing workout and its sets. Inserts any new sets and updates existing ones.
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

            // Checking for each set if set needs to be added newly or is an existing set and needs to be updated:
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
    
    
    // Creates a new workout and its associated sets, skipping any sets with zero reps or weight.
    public void CreateWorkout(Workout workout)
    {
        workout.Sets = workout.Sets.Where(set => set.Reps > 0 && set.Weight > 0).ToList();
        _connection.Execute(
            "INSERT INTO workouts (ExerciseName, WorkoutDate, Notes) VALUES (@exerciseName, @workoutdate, @notes);",
            new
            {
                exerciseName = workout.ExerciseName, sets = workout.Sets, workoutdate = workout.WorkoutDate, notes = workout.Notes
            });
        
        // Grabs last created workout ID to then create corresponding sets:
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

    // Deletes a workout and all of its associated sets from the database, using SQL stored procedures.
    public void DeleteWorkout(int id)
    {
        _connection.Execute("DeleteWorkoutAndSets",
            new { p_workoutId = id },
            commandType: CommandType.StoredProcedure);
        
        // WITHOUT STORED PROCEDURES:
        // _connection.Execute("DELETE FROM workout_sets WHERE WorkoutId = @workoutId;", new { workoutId = id });
        // _connection.Execute("DELETE FROM Workouts WHERE WorkoutId = @workoutId;", new { workoutId = id });
    }

    // Deletes a single set from the database using its set ID, using stored procedures.
    public void DeleteSet(int id)
    {
        _connection.Execute("DeleteSet",
            new { p_setId = id },
            commandType: CommandType.StoredProcedure);
        
        // WITHOUT STORED PROCEDURES:    
        // _connection.Execute("DELETE FROM workout_sets WHERE SetId = @setId;", new { setId = id });
    }

}