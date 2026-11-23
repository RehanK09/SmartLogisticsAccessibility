using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLogisticsAccessibility.Web.Models;

/// <summary>
/// Represents a shipment being tracked through the logistics platform.
/// </summary>
public class Shipment
{
    public int Id { get; set; }

    [Required, StringLength(30)]
    [Display(Name = "Tracking Number")]
    public string TrackingNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    [Display(Name = "Shipment Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Origin")]
    public int OriginLocationId { get; set; }

    [Required]
    [Display(Name = "Destination")]
    public int DestinationLocationId { get; set; }

    [Display(Name = "Status")]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

    [Display(Name = "Priority")]
    public ShipmentPriority Priority { get; set; } = ShipmentPriority.Normal;

    [Required]
    [Display(Name = "Sender Name")]
    [StringLength(120)]
    public string SenderName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Receiver Name")]
    [StringLength(120)]
    public string ReceiverName { get; set; } = string.Empty;

    [Display(Name = "Weight (kg)")]
    [Range(0.01, 10000)]
    public double? WeightKg { get; set; }

    [Display(Name = "Estimated Delivery Date")]
    [DataType(DataType.Date)]
    public DateTime? EstimatedDeliveryDate { get; set; }

    [Display(Name = "Actual Delivery Date")]
    [DataType(DataType.Date)]
    public DateTime? ActualDeliveryDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Flag to identify seeded demo/sample records.</summary>
    public bool IsSampleData { get; set; } = false;

    // Navigation properties
    public Location? OriginLocation { get; set; }
    public Location? DestinationLocation { get; set; }
    public Route? AssignedRoute { get; set; }
    public int? AssignedRouteId { get; set; }

    // Computed helpers
    [NotMapped]
    public string StatusBadgeClass => Status switch
    {
        ShipmentStatus.Pending => "badge-warning",
        ShipmentStatus.InTransit => "badge-info",
        ShipmentStatus.Delivered => "badge-success",
        ShipmentStatus.Delayed => "badge-danger",
        ShipmentStatus.Cancelled => "badge-secondary",
        _ => "badge-light"
    };

    [NotMapped]
    public string PriorityBadgeClass => Priority switch
    {
        ShipmentPriority.Low => "badge-secondary",
        ShipmentPriority.Normal => "badge-primary",
        ShipmentPriority.High => "badge-warning",
        ShipmentPriority.Critical => "badge-danger",
        _ => "badge-light"
    };
}

public enum ShipmentStatus
{
    Pending,
    [Display(Name = "In Transit")]
    InTransit,
    Delivered,
    Delayed,
    Cancelled
}

public enum ShipmentPriority
{
    Low,
    Normal,
    High,
    Critical
}
