document.getElementById("add-set-btn").addEventListener("click", function() {
    
    

    const newSet = document.createElement("div")

    newSet.innerHTML = 
    `<div class="set-template">
    <h4>Set ${setCount + 1}</h4><span class="delete-set-btn">&times;</span>

    <input type="hidden" id="newSetId" name="Sets[${setCount}].SetId" value="0" /> 

    <input type="hidden" name="Sets[${setCount}].SetNumber" value="${setCount}" class="form-control"/>

    <label>Reps</label>
    <input type="number" name="Sets[${setCount}].Reps" class="form-control"/>

    <label>Weight</label>
    <input type="number" name="Sets[${setCount}].Weight" class="form-control"/>

</div>`

    document.getElementById('sets-container').appendChild(newSet);
    setCount++;
})

document.addEventListener("click", async(e) => {
    if (e.target.classList.contains("delete-saved-set-btn")) {
        e.preventDefault();
    
        const confirmDelete = confirm("Are you sure you want to delete this set?");
        if (!confirmDelete) return;

        const setId = e.target.dataset.setId;
        const setElement = e.target.closest(".set-group");
        
        try {
            const response = await fetch(`/Workout/DeleteSet/${setId}`, {
                method: "POST",
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            });
            if (response.ok) {
                setElement.remove();
            } else {
                alert("Failed to delete this set.");
            }
        } catch (error) {
            console.error(error);
            alert("An error occurred.");
        }
    }
});

document.getElementById("sets-container").addEventListener("click", function (e) {
    if (e.target.classList.contains("delete-set-btn")) {
        e.preventDefault();
        e.target.closest(".set-template").remove();
        setCount--;
    }
});