using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();


//this will create the database if it does not exist. the scope is used to dispose of the context after the app has finished running.
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try 
{
    var context = services.GetRequiredService<AppDbContext>();  
    //migrateAsync will apply any pending migrations for the context to the database. if the database does not exist it will be created
    await context.Database.MigrateAsync();
    //this is a static method that will seed the database with data if there is no data in the database, no need to create an instance of the class
    await DbInitializer.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();


///when web app starts .net will look for configuration file appsettings.json and environment variables from the system . these are the default configuration sources
///if we want to add more configuration sources we can do so by using the ConfigurationBuilder class. Appsettings.Development.json is used when the app is running in development mode
///appsettings.json is used when the app is running in production mode.