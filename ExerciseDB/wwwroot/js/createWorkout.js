document.getElementById("add-set-btn").addEventListener("click", function() {

    

    const newSet = document.createElement("div")

    newSet.innerHTML = 
    `<div class="set-template">
    <h4>Set ${setCount + 1}</h4><span class="delete-set-btn">&times;</span>

    <input type="hidden" id="newSetId" name="Sets[${setCount}].SetId" value="0" /> 

    <input type="hidden" name="Sets[${setCount}].SetNumber" value="${setCount + 1}" class="form-control"/>

    <label>Reps</label>
    <input type="number" name="Sets[${setCount}].Reps" class="form-control"/>

    <label>Weight</label>
    <input type="number" name="Sets[${setCount}].Weight" class="form-control"/>
    
</div>`

    document.getElementById('sets-container').appendChild(newSet);
    
    setCount++;
})

document.getElementById("sets-container").addEventListener("click", function(e) {
    if (e.target.classList.contains("delete-saved-set-btn")) {
        const confirmed = confirm("Are you sure you want to delete this set?");
        if (!confirmed) return;
        
        // Hide the set you want to delete:
        const setId = e.target.dataset.setId;
        const setElement = e.target.closest(".set-group");
        if (setElement) setElement.remove();
        
        // Adds hidden input to track setIDs of sets to be deleted:
        const setsToDelete = document.createElement("input");
        setsToDelete.type = "hidden";
        setsToDelete.name = "DeletedSetIds";
        setsToDelete.value = setId;
        document.getElementById("deleted-set-ids").appendChild(setsToDelete);
    }
});

document.getElementById("sets-container").addEventListener("click", function (e) {
    if (e.target.classList.contains("delete-set-btn")) {
        e.preventDefault();
        e.target.closest(".set-template").remove();
        setCount--;
    }
});