using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Data;
using SmartLogisticsAccessibility.Web.Models;

namespace SmartLogisticsAccessibility.Web.Controllers;

/// <summary>
/// CRUD operations for Shipments.
/// </summary>
public class ShipmentsController : Controller
{
    private readonly ApplicationDbContext _db;

    public ShipmentsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Shipments
    public async Task<IActionResult> Index(string? search, string? status, string? priority)
    {
        var query = _db.Shipments
            .Include(s => s.OriginLocation)
            .Include(s => s.DestinationLocation)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(s => s.TrackingNumber.Contains(search)
                                  || s.Description.Contains(search)
                                  || s.SenderName.Contains(search)
                                  || s.ReceiverName.Contains(search));

        if (Enum.TryParse<ShipmentStatus>(status, out var statusEnum))
            query = query.Where(s => s.Status == statusEnum);

        if (Enum.TryParse<ShipmentPriority>(priority, out var priorityEnum))
            query = query.Where(s => s.Priority == priorityEnum);

        ViewBag.Search = search;
        ViewBag.Status = status;
        ViewBag.Priority = priority;

        var shipments = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
        return View(shipments);
    }

    // GET: /Shipments/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var shipment = await _db.Shipments
            .Include(s => s.OriginLocation)
            .Include(s => s.DestinationLocation)
            .Include(s => s.AssignedRoute)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null) return NotFound();
        return View(shipment);
    }

    // GET: /Shipments/Create
    public async Task<IActionResult> Create()
    {
        await PopulateViewBagsAsync();
        return View(new Shipment { TrackingNumber = $"SL-{DateTime.Now.Year}-{new Random().Next(1000, 9999)}" });
    }

    // POST: /Shipments/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Shipment shipment)
    {
        // Check unique tracking number
        if (await _db.Shipments.AnyAsync(s => s.TrackingNumber == shipment.TrackingNumber))
            ModelState.AddModelError(nameof(Shipment.TrackingNumber), "A shipment with this tracking number already exists.");

        if (ModelState.IsValid)
        {
            shipment.CreatedAt = DateTime.UtcNow;
            shipment.UpdatedAt = DateTime.UtcNow;
            _db.Shipments.Add(shipment);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Shipment {shipment.TrackingNumber} created successfully.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateViewBagsAsync(shipment);
        return View(shipment);
    }

    // GET: /Shipments/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var shipment = await _db.Shipments.FindAsync(id);
        if (shipment == null) return NotFound();
        await PopulateViewBagsAsync(shipment);
        return View(shipment);
    }

    // POST: /Shipments/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Shipment shipment)
    {
        if (id != shipment.Id) return BadRequest();

        if (await _db.Shipments.AnyAsync(s => s.TrackingNumber == shipment.TrackingNumber && s.Id != id))
            ModelState.AddModelError(nameof(Shipment.TrackingNumber), "A different shipment already uses this tracking number.");

        if (ModelState.IsValid)
        {
            shipment.UpdatedAt = DateTime.UtcNow;
            _db.Update(shipment);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Shipment {shipment.TrackingNumber} updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateViewBagsAsync(shipment);
        return View(shipment);
    }

    // GET: /Shipments/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var shipment = await _db.Shipments
            .Include(s => s.OriginLocation)
            .Include(s => s.DestinationLocation)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null) return NotFound();
        return View(shipment);
    }

    // POST: /Shipments/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var shipment = await _db.Shipments.FindAsync(id);
        if (shipment != null)
        {
            _db.Shipments.Remove(shipment);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Shipment {shipment.TrackingNumber} deleted.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateViewBagsAsync(Shipment? selected = null)
    {
        var locations = await _db.Locations.Where(l => l.IsActive).OrderBy(l => l.Name).AsNoTracking().ToListAsync();
        var routes = await _db.Routes.Where(r => r.IsActive).OrderBy(r => r.Name).AsNoTracking().ToListAsync();
        ViewBag.Locations = new SelectList(locations, "Id", "Name", selected?.OriginLocationId);
        ViewBag.DestinationLocations = new SelectList(locations, "Id", "Name", selected?.DestinationLocationId);
        ViewBag.Routes = new SelectList(routes, "Id", "Name", selected?.AssignedRouteId);
    }
}
