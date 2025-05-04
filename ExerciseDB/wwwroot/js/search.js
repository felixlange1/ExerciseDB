// The purpose of this script is to generate live search results when searching for exercises.

const searchInput = document.querySelector(".exercise-search"); // Search input field
const suggestionBox = document.getElementById("search-suggestions"); // Live search suggestions under input
const resultsContainer = document.getElementById("results-container"); // Container for search results
let exercises = []; // Array to store exercise search results

searchInput.addEventListener("input", async function (e) {
    
    const dataMode = searchInput.dataset.mode; // Grabs the data mode, depending on which view the search input is in 
    const query = e.target.value;
        
    if (query.length > 3) // Only starts live search if the user has entered more than 3 characters
    {
        const response = await fetch(`/Exercise/LiveSearch?searchTerm=${query}`);
        exercises = await response.json();
        
        suggestionBox.innerHTML = ""; 

        // Turns display of live search results on or off:
        if (exercises.length > 0)
        {
            suggestionBox.style.display = "block";
        }
        else {
            suggestionBox.style.display = "none";
        }
        
        // adds container for each exercise gotten from API:
        exercises.forEach((exercise, index) => {
            
            const listItem = document.createElement("div");
            
            listItem.textContent = exercise.name;
            listItem.classList.add("result-list");
            listItem.addEventListener("click", () => {
                // For clicking on a search result in Exercise Index View:
                if (dataMode === "full") {

                    resultsContainer.innerHTML= ""; 
                    displaySearchResults(exercises[index]); // displays full search results
                    suggestionBox.style.display = "none"; // turns off suggestion box after clicking on a result
                }
                // For clicking on a search result in Create Workout View:
                else if (dataMode === "form") {
                    document.getElementById("exercise-name").value = exercise.name;
                    suggestionBox.style.display = "none"; 
                    
                }
            });
            
            suggestionBox.appendChild(listItem);
    });
    }
    // clears search suggestions if search term is less than 3 characters:
    else {
        suggestionBox.innerHTML = "";
        suggestionBox.style.display = "none";
    }
});

// Creates new HTML to display full search results:
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

