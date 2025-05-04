<h1>Fitness Tracker Application</h1>

<h2>📌 Purpose</h2>
<p>
  This application allows users to search for and view exercises using an external API,
  then save those exercises as custom workouts. Users can create, edit, and delete workouts,
  set a custom number of sets, and sort their saved workouts by date or exercise name.
  Live search results enhance the user experience with real-time suggestions.
</p>

<h2>🚀 Features</h2>
<ul>
  <li>🔍 Search exercises using a third-party API (RapidAPI)</li>
  <li>📥 Save exercises to create custom workouts</li>
  <li>✏️ Edit and delete workouts</li>
  <li>📅 Sort workouts by date or exercise name</li>
  <li>📊 Choose a custom number of sets per workout</li>
  <li>⚡ Live search results powered by JavaScript</li>
</ul>

<h2>🛠 Technologies Used</h2>
<ul>
  <li>C#</li>
  <li>ASP.NET MVC Core</li>
  <li>JavaScript</li>
  <li>MySQL</li>
  <li>Dapper</li>
  <li>Bootstrap</li>
  <li>Custom CSS</li>
</ul>

<h2>📦 Installation &amp; Usage</h2>

<h3>Steps to Run Locally</h3>
<ol>
  <li><strong>Clone the repository:</strong>
    <pre><code>git clone https://github.com/yourusername/fitness-tracker.git</code></pre>
  </li>
  <li><strong>Navigate into the project folder:</strong>
    <pre><code>cd fitness-tracker</code></pre>
  </li>
  <li><strong>Restore dependencies:</strong>
    <pre><code>dotnet restore</code></pre>
  </li>
  <li><strong>Set up your MySQL database manually.</strong><br>
    Use MySQL Workbench (or similar) to create the database and tables needed.<br>
    Refer to the <code>Workout</code> and <code>Set</code> models in the codebase to structure your schema.
  </li>
  <li><strong>Configure your API key:</strong>
    <ul>
      <li>Sign up at <a href="https://rapidapi.com" target="_blank">RapidAPI</a> and subscribe to the <a href="https://rapidapi.com/justin-WFnsXH_t6/api/exercisedb" target="_blank">ExerciseDB API</a>.</li>
      <li>Create a file named <code>appsettings.json</code> (or <code>appsettings.Development.json</code>) with the following content:</li>
    </ul>
    <pre><code>{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=fitness_tracker_db;User=root;Password=yourpassword;"
  },
  "ApiSettings": {
    "ApiKey": "your-api-key-here"
  }
}</code></pre>

  </li>
  <li><strong>Run the project:</strong>
    <pre><code>dotnet run</code></pre>
  </li>

</ol>
<h3>Steps to Run with Rider (Alternative)</h3>
<p>If you're using <strong>Rider</strong>, you can skip the terminal commands and follow these steps:</p>
<ol>
  <li><strong>Open the project:</strong> Launch Rider and open the cloned project folder.</li>
  <li><strong>Restore Dependencies:</strong> Rider will automatically restore the necessary dependencies when you open the project.</li>
  <li><strong>Run the application:</strong> Click the green "Run" button to start the application. Rider will automatically launch the project in your default browser.</li>
  
</ol>


<h2>🙌 Acknowledgements</h2>
<p>Exercise data provided by <a href="https://rapidapi.com/justin-WFnsXH_t6/api/exercisedb" target="_blank">ExerciseDB API on RapidAPI</a></p>
