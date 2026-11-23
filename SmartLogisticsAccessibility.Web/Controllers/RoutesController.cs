using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Data;
using SmartLogisticsAccessibility.Web.Models;
using SmartLogisticsAccessibility.Web.Services;
using LogisticsRoute = SmartLogisticsAccessibility.Web.Models.Route;

namespace SmartLogisticsAccessibility.Web.Controllers;

/// <summary>
/// Route management and intelligent recommendation engine.
/// </summary>
public class RoutesController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly RouteRecommendationService _recommender;

    public RoutesController(ApplicationDbContext db, RouteRecommendationService recommender)
    {
        _db = db;
        _recommender = recommender;
    }

    // GET: /Routes
    public async Task<IActionResult> Index()
    {
        var routes = await _db.Routes
            .Include(r => r.OriginLocation)
            .Include(r => r.DestinationLocation)
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync();
        return View(routes);
    }

    // GET: /Routes/Intelligence — The main route recommendation page
    public async Task<IActionResult> Intelligence(int? originId, int? destinationId, ShipmentPriority priority = ShipmentPriority.Normal)
    {
        await PopulateLocationsAsync();
        ViewBag.Priority = priority;
        ViewBag.OriginId = originId;
        ViewBag.DestinationId = destinationId;

        if (originId == null || destinationId == null)
            return View("Intelligence", new List<SmartLogisticsAccessibility.Web.Services.RouteRecommendation>());

        var matchingRoutes = await _db.Routes
            .Include(r => r.OriginLocation)
            .Include(r => r.DestinationLocation)
            .Where(r => r.IsActive && r.OriginLocationId == originId && r.DestinationLocationId == destinationId)
            .AsNoTracking()
            .ToListAsync<LogisticsRoute>();

        var recommendations = _recommender.Recommend(matchingRoutes, priority);
        ViewBag.RouteCount = matchingRoutes.Count;
        ViewBag.OriginName = matchingRoutes.FirstOrDefault()?.OriginLocation?.Name ?? "—";
        ViewBag.DestinationName = matchingRoutes.FirstOrDefault()?.DestinationLocation?.Name ?? "—";

        return View("Intelligence", recommendations);
    }

    // GET: /Routes/Create
    public async Task<IActionResult> Create()
    {
        await PopulateLocationsAsync();
        return View(new LogisticsRoute());
    }

    // POST: /Routes/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LogisticsRoute route)
    {
        if (ModelState.IsValid)
        {
            route.CreatedAt = DateTime.UtcNow;
            _db.Routes.Add(route);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Route \"{route.Name}\" created.";
            return RedirectToAction(nameof(Index));
        }
        await PopulateLocationsAsync();
        return View(route);
    }

    // GET: /Routes/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var route = await _db.Routes.FindAsync(id);
        if (route == null) return NotFound();
        await PopulateLocationsAsync();
        return View(route);
    }

    // POST: /Routes/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LogisticsRoute route)
    {
        if (id != route.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            _db.Update(route);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Route \"{route.Name}\" updated.";
            return RedirectToAction(nameof(Index));
        }
        await PopulateLocationsAsync();
        return View(route);
    }

    // GET: /Routes/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var route = await _db.Routes
            .Include(r => r.OriginLocation)
            .Include(r => r.DestinationLocation)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
        if (route == null) return NotFound();
        return View(route);
    }

    // POST: /Routes/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var route = await _db.Routes.FindAsync(id);
        if (route != null)
        {
            _db.Routes.Remove(route);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Route \"{route.Name}\" deleted.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLocationsAsync()
    {
        var locations = await _db.Locations.Where(l => l.IsActive).OrderBy(l => l.Name).AsNoTracking().ToListAsync();
        ViewBag.Locations = new SelectList(locations, "Id", "Name");
    }
}
