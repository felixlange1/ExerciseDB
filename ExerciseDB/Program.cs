// This uses top-level statements, the default style in ASP.NET Core 6 and later,
// so there is no explicit Main method.

using System.Data;
using ExerciseDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Determine connection string (local vs Azure):
var azureConn = builder.Configuration.GetConnectionString("MYSQLCONNSTR_localdb");
string connString;
if (!string.IsNullOrEmpty(azureConn))
{
    connString = azureConn;
}

else
{
    connString = builder.Configuration.GetConnectionString("exercise");
}


// For EF Core / Identity:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connString, ServerVersion.AutoDetect(connString)));

// For Dapper:

builder.Services.AddScoped<IDbConnection>((s) =>
{
    // IDbConnection conn = new SqlConnection(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
    IDbConnection conn = new MySqlConnection(connString);
    conn.Open();
    return conn;
});


// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddScoped<IDbConnection>((s) =>
// {
//     IDbConnection conn = new MySqlConnection(builder.Configuration.GetConnectionString("exercise"));
//     conn.Open();
//     return conn;
// });





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


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
