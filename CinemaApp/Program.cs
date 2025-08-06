using CinemaApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adaugă suport pentru MVC
builder.Services.AddControllersWithViews();

// Adaugă conexiunea la SQL Server
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Middleware standard
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware aplicație
// app.UseHttpsRedirection(); // Activează dacă folosești HTTPS
app.UseStaticFiles();
app.UseRouting();

// Rute implicite
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();
