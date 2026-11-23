using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLogisticsAccessibility.Web.Models;

/// <summary>
/// Represents a physical location such as a warehouse, hub or delivery address.
/// </summary>
public class Location
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Location Name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string State { get; set; } = string.Empty;

    [StringLength(20)]
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    /// <summary>Latitude for map display (decimal degrees).</summary>
    [Range(-90, 90)]
    public double? Latitude { get; set; }

    /// <summary>Longitude for map display (decimal degrees).</summary>
    [Range(-180, 180)]
    public double? Longitude { get; set; }

    [Display(Name = "Location Type")]
    public LocationType Type { get; set; } = LocationType.Warehouse;

    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Marks records created by DbSeeder as sample/demo data.</summary>
    public bool IsSampleData { get; set; } = false;

    // Navigation properties
    public ICollection<Shipment> OriginShipments { get; set; } = new List<Shipment>();
    public ICollection<Shipment> DestinationShipments { get; set; } = new List<Shipment>();
    public ICollection<AccessibilityReport> AccessibilityReports { get; set; } = new List<AccessibilityReport>();
    public ICollection<Route> RoutesAsOrigin { get; set; } = new List<Route>();
    public ICollection<Route> RoutesAsDestination { get; set; } = new List<Route>();

    // Computed helper
    [NotMapped]
    public string FullAddress => $"{Address}, {City}, {State}" + (PostalCode != null ? $" - {PostalCode}" : "");

    [NotMapped]
    public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;
}

public enum LocationType
{
    Warehouse,
    Hub,
    DeliveryPoint,
    Office,
    Port,
    Airport
}
