using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddLogging(); // This enables ILogger
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

// Register your services
builder.Services.AddSingleton<INotesService, NotesService>();
builder.Services.AddSingleton<IHabitService, HabitService>();
builder.Services.AddSingleton<ICalendarService, CalendarService>();
builder.Services.AddSingleton<IMeditationService, MeditationService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions {
    ContentTypeProvider = new FileExtensionContentTypeProvider {
        Mappings = { [".mp3"] = "audio/mpeg" }
    }
});
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
