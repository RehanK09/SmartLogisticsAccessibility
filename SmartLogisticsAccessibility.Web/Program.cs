using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Data;
using SmartLogisticsAccessibility.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- Services ----------------------------------------------------------------
builder.Services.AddControllersWithViews();

// Entity Framework Core — SQL Server connection configured in appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the route recommendation service (stateless, registered as singleton)
builder.Services.AddSingleton<RouteRecommendationService>();

var app = builder.Build();

// ---- Middleware pipeline ------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// ---- Apply migrations and seed data on startup -------------------------------
// This runs EF Core migrations automatically and seeds sample data if the DB is empty.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Apply any pending migrations automatically
        await db.Database.MigrateAsync();
        // Seed the sample data
        await DbSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration or seeding. " +
            "Make sure SQL Server Express is running and the connection string in appsettings.json is correct.");
    }
}

app.Run();
