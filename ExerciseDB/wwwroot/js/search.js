const searchInput = document.getElementById("search-input");
const suggestionBox = document.getElementById("search-suggestions");
let exercises = [];

searchInput.addEventListener("input", async function (e) {
    
    const query = e.target.value;
        
    if (query.length > 3)
    {
        const response = await fetch(`/Exercise/LiveSearch?searchTerm=${query}`);
        exercises = await response.json();
        
        suggestionBox.innerHTML = ""; 

        if (exercises.length > 0)
        {
            suggestionBox.style.display = "block";
        }
        else {
            suggestionBox.style.display = "none";
        }
        
        exercises.forEach((exercise, index) => {
            capitalizedName = toTitleCase(exercise.name);
            const listItem = document.createElement("div");
            
            listItem.textContent = capitalizedName;
            listItem.classList.add("result-list");
            listItem.addEventListener("click", () => {
                suggestionBox.innerHTML = "";
                displaySearchResults(exercises[index], capitalizedName);
            });
            suggestionBox.appendChild(listItem);
    });
    }
    else {
        suggestionBox.innerHTML = "";
        suggestionBox.style.display = "none";
    }
});

function displaySearchResults(exercise, capitalizedName) {
    const resultsContainer = document.getElementById("results-container");
    resultsContainer.innerHTML = `
        <div class="d-flex justify-content-between align-items-center">
        <h3>${toTitleCase(exercise.name)}</h3>
        <a href="/Workout/CreateWorkout/?exerciseName=${capitalizedName}" class="btn btn-primary">Save as workout</a></div>
        <p><strong>Body Part: </strong>${exercise.bodyPart}</p>
        <p><strong>Target Muscle: </strong>${exercise.target}</p>
        <p><strong>Equipment Used: </strong>${exercise.equipment}</p>
        <img src="${exercise.gifUrl}" alt="${exercise.name}" width="200" height="200">
        <p><strong>Instructions:</strong></p>
        <ul>
            ${exercise.instructions.map(step => `<li>${step}</li>`).join('')}
        </ul>
    `;
    }


function toTitleCase(string) {
    return string.replace(/\b\w/g, char => char.toUpperCase());
}