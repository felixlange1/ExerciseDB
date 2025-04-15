document.getElementById("add-set-btn").addEventListener("click", function() {
    
    

    const newSet = document.createElement("div")

    newSet.innerHTML = 
    `<h4>Set ${setCount}</h4>

    <input type="hidden" id="newSetId" name="Sets[${setCount}].SetId" value="0" /> 

    <input type="hidden" name="Sets[${setCount}].SetNumber" value="${setCount}" class="form-control"/>

    <label>Reps</label>
    <input type="number" name="Sets[${setCount}].Reps" class="form-control"/>

    <label>Weight</label>
    <input type="number" name="Sets[${setCount}].Weight" class="form-control"/>`

    document.getElementById('sets-container').appendChild(newSet);
    setCount++;
})