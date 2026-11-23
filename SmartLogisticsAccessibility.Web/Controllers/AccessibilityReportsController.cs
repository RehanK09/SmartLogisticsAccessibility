using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Data;
using SmartLogisticsAccessibility.Web.Models;

namespace SmartLogisticsAccessibility.Web.Controllers;

/// <summary>
/// CRUD operations for Accessibility Reports.
/// </summary>
public class AccessibilityReportsController : Controller
{
    private readonly ApplicationDbContext _db;
    public AccessibilityReportsController(ApplicationDbContext db) { _db = db; }

    // GET: /AccessibilityReports
    public async Task<IActionResult> Index(string? search, string? rating)
    {
        var query = _db.AccessibilityReports
            .Include(r => r.Location)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Title.Contains(search) || r.ReporterName.Contains(search));

        var list = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();

        // Apply computed Rating filter in memory (since AccessibilityScore is computed)
        if (!string.IsNullOrWhiteSpace(rating) && Enum.TryParse<AccessibilityRating>(rating, out var ratingEnum))
            list = list.Where(r => r.Rating == ratingEnum).ToList();

        ViewBag.Search = search;
        ViewBag.Rating = rating;
        return View(list);
    }

    // GET: /AccessibilityReports/Map
    public async Task<IActionResult> Map()
    {
        var locations = await _db.Locations
            .Include(l => l.AccessibilityReports)
            .Where(l => l.Latitude.HasValue && l.Longitude.HasValue)
            .AsNoTracking()
            .ToListAsync();
        return View(locations);
    }

    // GET: /AccessibilityReports/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var report = await _db.AccessibilityReports
            .Include(r => r.Location)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
        if (report == null) return NotFound();
        return View(report);
    }

    // GET: /AccessibilityReports/Create
    public async Task<IActionResult> Create()
    {
        await PopulateLocations();
        return View(new AccessibilityReport());
    }

    // POST: /AccessibilityReports/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccessibilityReport report)
    {
        if (ModelState.IsValid)
        {
            report.CreatedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;
            _db.AccessibilityReports.Add(report);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Accessibility report \"{report.Title}\" submitted successfully.";
            return RedirectToAction(nameof(Index));
        }
        await PopulateLocations(report.LocationId);
        return View(report);
    }

    // GET: /AccessibilityReports/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var report = await _db.AccessibilityReports.FindAsync(id);
        if (report == null) return NotFound();
        await PopulateLocations(report.LocationId);
        return View(report);
    }

    // POST: /AccessibilityReports/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AccessibilityReport report)
    {
        if (id != report.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            report.UpdatedAt = DateTime.UtcNow;
            _db.Update(report);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Accessibility report updated.";
            return RedirectToAction(nameof(Index));
        }
        await PopulateLocations(report.LocationId);
        return View(report);
    }

    // GET: /AccessibilityReports/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var report = await _db.AccessibilityReports.Include(r => r.Location).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (report == null) return NotFound();
        return View(report);
    }

    // POST: /AccessibilityReports/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var report = await _db.AccessibilityReports.FindAsync(id);
        if (report != null)
        {
            _db.AccessibilityReports.Remove(report);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Accessibility report deleted.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLocations(int? selectedId = null)
    {
        var locs = await _db.Locations.Where(l => l.IsActive).OrderBy(l => l.Name).AsNoTracking().ToListAsync();
        ViewBag.Locations = new SelectList(locs, "Id", "Name", selectedId);
    }
}
