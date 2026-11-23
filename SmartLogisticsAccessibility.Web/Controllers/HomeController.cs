using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Data;
using SmartLogisticsAccessibility.Web.Models;
using SmartLogisticsAccessibility.Web.ViewModels;
using System.Text.Json;

namespace SmartLogisticsAccessibility.Web.Controllers;

/// <summary>
/// Handles the main dashboard — shows summary cards and charts.
/// </summary>
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var shipments = await _db.Shipments.AsNoTracking().ToListAsync();
        var reports = await _db.AccessibilityReports.AsNoTracking().ToListAsync();

        var vm = new DashboardViewModel
        {
            TotalShipments = shipments.Count,
            PendingShipments = shipments.Count(s => s.Status == ShipmentStatus.Pending),
            InTransitShipments = shipments.Count(s => s.Status == ShipmentStatus.InTransit),
            DeliveredShipments = shipments.Count(s => s.Status == ShipmentStatus.Delivered),
            DelayedShipments = shipments.Count(s => s.Status == ShipmentStatus.Delayed),

            TotalAccessibilityReports = reports.Count,
            FullyAccessibleCount = reports.Count(r => r.Rating == AccessibilityRating.FullyAccessible),
            PartiallyAccessibleCount = reports.Count(r => r.Rating == AccessibilityRating.PartiallyAccessible),
            HasIssuesCount = reports.Count(r => r.Rating == AccessibilityRating.HasIssues),

            TotalLocations = await _db.Locations.CountAsync(),
            TotalRoutes = await _db.Routes.CountAsync(),

            RecentShipments = await _db.Shipments
                .Include(s => s.OriginLocation)
                .Include(s => s.DestinationLocation)
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .AsNoTracking()
                .ToListAsync(),

            RecentReports = await _db.AccessibilityReports
                .Include(r => r.Location)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .AsNoTracking()
                .ToListAsync(),
        };

        // Build Chart.js data
        var statusGroups = shipments
            .GroupBy(s => s.Status.ToString())
            .Select(g => new { Label = g.Key, Count = g.Count() })
            .ToList();
        vm.ShipmentStatusLabelsJson = JsonSerializer.Serialize(statusGroups.Select(x => x.Label).ToList());
        vm.ShipmentStatusDataJson   = JsonSerializer.Serialize(statusGroups.Select(x => x.Count).ToList());

        vm.AccessibilityRatingLabelsJson = JsonSerializer.Serialize(new[] { "Fully Accessible", "Partially Accessible", "Has Issues" });
        vm.AccessibilityRatingDataJson   = JsonSerializer.Serialize(new[] { vm.FullyAccessibleCount, vm.PartiallyAccessibleCount, vm.HasIssuesCount });

        return View(vm);
    }

    public IActionResult About() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
