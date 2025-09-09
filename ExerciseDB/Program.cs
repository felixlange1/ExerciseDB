// This uses top-level statements, the default style in ASP.NET Core 6 and later,
// so there is no explicit Main method.

using System.Data;
using ExerciseDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddScoped<IDbConnection>((s) =>
// {
//     IDbConnection conn = new MySqlConnection(builder.Configuration.GetConnectionString("exercise"));
//     conn.Open();
//     return conn;
// });

builder.Services.AddScoped<IDbConnection>((s) =>
{
    var connString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
    if (string.IsNullOrEmpty(connString))
        throw new Exception("AZURE_SQL_CONNECTIONSTRING environment variable not set!");
//    IDbConnection conn = new SqlConnection(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
    IDbConnection conn = new SqlConnection(connString);
    conn.Open();
    return conn;
});


builder.Services.AddTransient<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddHttpClient<ApiService>();




// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExerciseDB", Version = "v1" });
//     c.EnableAnnotations();
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

// app.UseSwagger();
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExerciseDB v1");
//     c.RoutePrefix = "swagger";
// });


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
