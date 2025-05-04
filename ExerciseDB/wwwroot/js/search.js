const searchInput = document.querySelector(".exercise-search");
const suggestionBox = document.getElementById("search-suggestions");
const resultsContainer = document.getElementById("results-container");
let exercises = [];

searchInput.addEventListener("input", async function (e) {
    
    const dataMode = searchInput.dataset.mode;
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
            
            const listItem = document.createElement("div");
            
            listItem.textContent = exercise.name;
            listItem.classList.add("result-list");
            listItem.addEventListener("click", () => {
                if (dataMode === "full") {

                    resultsContainer.innerHTML= "";
                    displaySearchResults(exercises[index]);
                    suggestionBox.style.display = "none";
                }
                else if (dataMode === "form") {
                    document.getElementById("exercise-name").value = exercise.name;
                    suggestionBox.style.display = "none";
                    
                }
            });
            suggestionBox.appendChild(listItem);
    });
    }
    else {
        suggestionBox.innerHTML = "";
        suggestionBox.style.display = "none";
    }
});

function displaySearchResults(exercise) {
    
    resultsContainer.innerHTML = `
        <div class="card shadow-sm rounded-3 mb-3">
            <div class="card-body">
                <div class="d-flex justify-content-between align-items-center">
                    <h3 class="card-title">${exercise.name}</h3>
                    <a href="/Workout/CreateWorkout/?exerciseName=${exercise.name}" class="btn btn-primary">Save as workout</a>
                </div>
                <p><strong>Body Part: </strong>${exercise.bodyPart}</p>
                <p><strong>Target Muscle: </strong>${exercise.target}</p>
                <p><strong>Equipment Used: </strong>${exercise.equipment}</p>
                <img src="${exercise.gifUrl}" alt="${exercise.name}" width="200" height="200">
                <p><strong>Instructions:</strong></p>
                <ul>
                    ${exercise.instructions.map(step => `<li>${step}</li>`).join('')}
                </ul>
            </div>
        </div>
    `;
    }


// function toTitleCase(string) {
//     return string.replace(/\b\w/g, char => char.toUpperCase());
// }