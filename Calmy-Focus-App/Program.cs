using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings from appsettings.json.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Register your existing services.
builder.Services.AddSingleton<INotesService, NotesService>();
builder.Services.AddSingleton<IHabitService, HabitService>();

// Register additional services for authentication.
// (Implementations for IUserService and UserService will be provided in later steps.)
builder.Services.AddSingleton<IUserService, UserService>();

// Register the password hasher for the User model.
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

// Add and configure Cookie Authentication.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Redirect to this path if the user is not authenticated.
        options.LoginPath = "/Account/Login";
        // Redirect here for logouts.
        options.LogoutPath = "/Account/Logout";
        // Redirect here when access is denied.
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddSingleton<IMeditationService, MeditationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions {
    ContentTypeProvider = new FileExtensionContentTypeProvider {
        Mappings = { [".mp3"] = "audio/mpeg" }
    }
});

app.UseRouting();

// IMPORTANT: Add the Authentication middleware BEFORE Authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();