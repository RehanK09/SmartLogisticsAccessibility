using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLogisticsAccessibility.Web.Models;

/// <summary>
/// A route option between two locations that can be scored and recommended.
/// All distance and time values are estimates used for rule-based comparison.
/// </summary>
public class Route
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Route Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Origin Location")]
    public int OriginLocationId { get; set; }

    [Required]
    [Display(Name = "Destination Location")]
    public int DestinationLocationId { get; set; }

    /// <summary>Estimated distance in kilometres (used in scoring).</summary>
    [Display(Name = "Estimated Distance (km)")]
    [Range(0.1, 100000)]
    public double EstimatedDistanceKm { get; set; }

    /// <summary>Estimated travel time in hours (used in scoring).</summary>
    [Display(Name = "Estimated Travel Time (hours)")]
    [Range(0.1, 500)]
    public double EstimatedTravelTimeHours { get; set; }

    [Display(Name = "Route Type")]
    public RouteType Type { get; set; } = RouteType.Road;

    [Display(Name = "Accessibility Level")]
    public AccessibilityLevel AccessibilityLevel { get; set; } = AccessibilityLevel.Partial;

    [Display(Name = "Cost Estimate (₹)")]
    [Range(0, 10000000)]
    public decimal? EstimatedCostINR { get; set; }

    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsSampleData { get; set; } = false;

    // Navigation
    public Location? OriginLocation { get; set; }
    public Location? DestinationLocation { get; set; }
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    // Computed
    [NotMapped]
    public string AccessibilityLevelBadgeClass => AccessibilityLevel switch
    {
        AccessibilityLevel.Full => "badge-success",
        AccessibilityLevel.Partial => "badge-warning",
        AccessibilityLevel.Limited => "badge-danger",
        _ => "badge-secondary"
    };
}

public enum RouteType
{
    Road,
    Rail,
    Air,
    Sea,
    [Display(Name = "Multi-Modal")]
    MultiModal
}

public enum AccessibilityLevel
{
    Full,
    Partial,
    Limited
}
