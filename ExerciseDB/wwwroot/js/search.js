const searchInput = document.getElementById("search-input");
let exercises = [];
searchInput.addEventListener("input", async function (e) {

    
    const query = e.target.value;
    
    if (query.length > 3)
    {
        const response = await fetch(`/Exercise/LiveSearch?searchTerm=${searchInput.value}`);
        exercises = await response.json();
        
        const resultsDiv = document.getElementById("results-container");
        resultsDiv.innerHTML = ""; 

        exercises.forEach((exercise, index) => {
            const link = document.createElement("a");
            const listItem = document.createElement("li");
            const list = document.createElement("ul");
            link.href = "javascript:void(0);";
            link.textContent = exercise.name;

            link.addEventListener("click", () => {
                displaySearchResults(exercises[index]);
            });
            
            resultsDiv.appendChild(list);
            list.appendChild(listItem);
            listItem.appendChild(link);
    
    });
    }
});

function displaySearchResults(exercise) {
    const resultsContainer = document.getElementById("results-container");
    resultsContainer.innerHTML = `
        <h3>${exercise.name}</h3>
        <p><strong>Body Part: </strong>${exercise.bodyPart}</p>
        <p><strong>Target Muscle: </strong>${exercise.target}</p>
        <p><strong>Equipment Used: </strong>${exercise.equipment}</p>
        <img src="${exercise.gifUrl}" alt="${exercise.name}" width="200" height="200">
        <h4>Instructions:</h4>
        <ul>
            ${exercise.instructions.map(step => `<li>${step}</li>`).join('')}
        </ul>
    `;
    }


// function sendExerciseDetailsToController(index) {
//     const exercise = exercises[index]; // Get the clicked exercise's full data
//
//     // Send the full exercise data to the controller (via POST)
//     fetch('/Exercise/ViewExercise', {
//         method: 'POST',
//         headers: {
//             'Content-Type': 'application/json',
//         },
//         body: JSON.stringify(exercise)
//     })
//         .then(response => response.text())
//         .then(html => {
//             document.getElementById("results-container").innerHTML = html;
//            
//         })
//         .catch(error => console.error('Error:', error));
// }
    





    
    // if (query.length > 3) {
    //     fetch(`/Exercise/LiveSearch?searchTerm=${searchInput.value}`)
    //         .then(response => response.json())
    //         .then(exercises => {
    //             results = exercises;
    //             displaySearchResults(exercises);
    //         })
    //         .catch(error => console.log('Error fetching exercises:', error));
    // }




// document.getElementById("results-container").addEventListener("click", function (e) {
//     if (e.target.classList.contains("exercise-item")) {
//         const exerciseId = e.target.getAttribute("data-id");
//         const selectedExercise = results[exerciseId];
//        
//         displayExercise(selectedExercise);
//     }
// })

