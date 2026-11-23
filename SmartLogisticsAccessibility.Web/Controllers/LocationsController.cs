using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Data;
using SmartLogisticsAccessibility.Web.Models;

namespace SmartLogisticsAccessibility.Web.Controllers;

/// <summary>
/// CRUD operations for Locations.
/// </summary>
public class LocationsController : Controller
{
    private readonly ApplicationDbContext _db;
    public LocationsController(ApplicationDbContext db) { _db = db; }

    // GET: /Locations
    public async Task<IActionResult> Index(string? search)
    {
        var query = _db.Locations
            .Include(l => l.AccessibilityReports)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(l => l.Name.Contains(search) || l.City.Contains(search) || l.State.Contains(search));

        ViewBag.Search = search;
        var list = await query.OrderBy(l => l.Name).ToListAsync();
        return View(list);
    }

    // GET: /Locations/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var location = await _db.Locations
            .Include(l => l.AccessibilityReports)
            .Include(l => l.OriginShipments)
            .Include(l => l.DestinationShipments)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
        if (location == null) return NotFound();
        return View(location);
    }

    // GET: /Locations/Create
    public IActionResult Create()
    {
        ViewBag.Types = GetTypeSelectList();
        return View(new Location());
    }

    // POST: /Locations/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Location location)
    {
        if (ModelState.IsValid)
        {
            location.CreatedAt = DateTime.UtcNow;
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Location \"{location.Name}\" added successfully.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Types = GetTypeSelectList(location.Type);
        return View(location);
    }

    // GET: /Locations/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location == null) return NotFound();
        ViewBag.Types = GetTypeSelectList(location.Type);
        return View(location);
    }

    // POST: /Locations/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Location location)
    {
        if (id != location.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            _db.Update(location);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Location \"{location.Name}\" updated.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Types = GetTypeSelectList(location.Type);
        return View(location);
    }

    // GET: /Locations/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var location = await _db.Locations.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        if (location == null) return NotFound();
        return View(location);
    }

    // POST: /Locations/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location != null)
        {
            _db.Locations.Remove(location);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Location \"{location.Name}\" deleted.";
        }
        return RedirectToAction(nameof(Index));
    }

    private static SelectList GetTypeSelectList(LocationType? selected = null)
        => new SelectList(Enum.GetValues<LocationType>().Select(e => new { Value = e, Text = e.ToString() }), "Value", "Text", selected);
}
