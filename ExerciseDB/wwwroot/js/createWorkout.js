
let setCount = 3
document.getElementById("add-set-btn").addEventListener("click", function() {
    
    setCount++;

    const newSet = document.createElement("div")

    newSet.innerHTML = 
    `<h4>Set ${setCount}</h4>

    <input type="hidden" name="WorkoutSets[${setCount}.SetNumber" value="${setCount}" className="form-control"/>

    <label>Reps</label>
    <input type="number" name="WorkoutSets[${setCount}].Reps" className="form-control"/>

    <label>Weight</label>
    <input type="number" name="WorkoutSets[${setCount}].Weight" className="form-control"/>`

    document.getElementById('sets-container').appendChild(newSet);
    
})