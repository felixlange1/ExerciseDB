// This adds sets when creating or updating a workout:
document.getElementById("add-set-btn").addEventListener("click", function() {

    

    const newSet = document.createElement("div")

    // HTML for a new set:
    newSet.innerHTML = 
    `<div class="row me-2">
        <div class="set-template ms-2">
            <div class="d-flex justify-content-between align-items-center">
                <h3 class="set-heading">Set ${setCount + 1}</h3>
                <button type="button" class="btn-close delete-set-btn mb-2"></button>
            </div>
    
        <input type="hidden" id="newSetId" name="Sets[${setCount}].SetId" value="0" /> 

        <input type="hidden" name="Sets[${setCount}].SetNumber" value="${setCount + 1}" class="form-control"/>

            <div class="row">
                 <div class="form-group col-md-2">
                    <label>Weight</label>
                    <input type="number" name="Sets[${setCount}].Weight" class="form-control"/>
                </div>
                <div class="form-group col-md-2">
                    <label>Reps</label>
                    <input type="number" name="Sets[${setCount}].Reps" class="form-control"/>
                </div>
            </div>
        </div>
    </div>`

    document.getElementById('sets-container').appendChild(newSet);
    
    setCount++;
})


// Using event delegation here since the Delete Set Button initially only exists for sets already saved in the database,
// not the dynamically added ones, so we're attaching addEventListener to the parent container:
document.getElementById("sets-container").addEventListener("click", function(e) {

    if (e.target.classList.contains("delete-saved-set-btn")) {
        const confirmed = confirm("Are you sure you want to delete this set?");
        if (!confirmed) return;
        
        // Hide the set you want to delete:
        const setId = e.target.dataset.setId;
        const setElement = e.target.closest(".set-group");
        if (setElement) setElement.remove();
        
        // Adds hidden input to track setIDs of sets to be deleted, so it can then be submitted to the controller when
        // submitting the form:
        const setsToDelete = document.createElement("input");
        setsToDelete.type = "hidden";
        setsToDelete.name = "DeletedSetIds";
        setsToDelete.value = setId;
        document.getElementById("deleted-set-ids").appendChild(setsToDelete);
    }
});

// There's a second type of Delete Set button for handling unsaved, newly added sets. There's no need to keep track of setId,
// etc. and submit this to the controller:
document.getElementById("sets-container").addEventListener("click", function (e) {
    if (e.target.classList.contains("delete-set-btn")) {
        e.preventDefault();
        e.target.closest(".set-template").remove();
        setCount--;
    }
});

// The below code is for the Create Workout form, so the date is automatically today's date:

const dateInput = document.getElementById("workout-date");
const today = new Date().toLocaleDateString('en-CA');
dateInput.value = today;