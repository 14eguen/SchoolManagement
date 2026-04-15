using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireTeacher", policy => policy.RequireRole("Admin", "Teacher"));
});

builder.Services.AddRazorPages(options =>
{
    // Admin-only sections
    options.Conventions.AuthorizeFolder("/Users", "RequireAdmin");
    options.Conventions.AuthorizeFolder("/Teachers", "RequireAdmin");

    // Admin & Teacher sections (Students cannot access)
    options.Conventions.AuthorizeFolder("/Students", "RequireTeacher");
    options.Conventions.AuthorizeFolder("/Classes", "RequireTeacher");
    options.Conventions.AuthorizeFolder("/Subjects", "RequireTeacher");

    // Grades: all authenticated users can view, but only Admin/Teacher can modify
    options.Conventions.AuthorizeFolder("/Grades");
    options.Conventions.AuthorizePage("/Grades/Create", "RequireTeacher");
    options.Conventions.AuthorizePage("/Grades/Edit", "RequireTeacher");
    options.Conventions.AuthorizePage("/Grades/Delete", "RequireTeacher");

    // Timetables: all authenticated users can view, but only Admin/Teacher can modify
    options.Conventions.AuthorizeFolder("/Timetables");
    options.Conventions.AuthorizePage("/Timetables/Create", "RequireTeacher");
    options.Conventions.AuthorizePage("/Timetables/Edit", "RequireTeacher");
    options.Conventions.AuthorizePage("/Timetables/Delete", "RequireTeacher");

    // Role-specific dashboards
    options.Conventions.AuthorizeFolder("/Teacher", "RequireTeacher");
    options.Conventions.AuthorizeFolder("/Student");  // any logged-in user in student folder
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

var connectionString = builder.Configuration.GetConnectionString("DBDefault") 
    ?? throw new InvalidOperationException("Connection string 'DBDefault' not found.");

builder.Services.AddDbContext<SchoolManagementContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
