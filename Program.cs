using ChirayuHospitalMVC.Data;
using ChirayuHospitalMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// QuestPDF (if used)
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC + Views
builder.Services.AddControllersWithViews();

// IHttpContextAccessor (for session access in views)
builder.Services.AddHttpContextAccessor();


// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ================================
// SEED DEFAULT ADMIN ACCOUNT
// ================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var hasher = new PasswordHasher<User>();

    // Ensure database is created
    context.Database.Migrate();

    // If no admin exists, create one
    if (!context.Users.Any(u => u.Role == UserRole.Admin))
    {
        var admin = new User
        {
            Username = "admin" +
            "",
            Email = "admin@hospital.com",
            Role = UserRole.Admin
        };

        admin.PasswordHash = hasher.HashPassword(admin, "admin123");

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

// ================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();