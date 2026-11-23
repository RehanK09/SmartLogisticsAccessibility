using SmartLogisticsAccessibility.Web.Models;

namespace SmartLogisticsAccessibility.Web.ViewModels;

/// <summary>Summary card data for the dashboard.</summary>
public class DashboardViewModel
{
    // Shipment summary
    public int TotalShipments { get; set; }
    public int PendingShipments { get; set; }
    public int InTransitShipments { get; set; }
    public int DeliveredShipments { get; set; }
    public int DelayedShipments { get; set; }

    // Accessibility summary
    public int TotalAccessibilityReports { get; set; }
    public int FullyAccessibleCount { get; set; }
    public int PartiallyAccessibleCount { get; set; }
    public int HasIssuesCount { get; set; }

    // Other
    public int TotalLocations { get; set; }
    public int TotalRoutes { get; set; }

    // Recent data for dashboard tables
    public List<Shipment> RecentShipments { get; set; } = new();
    public List<AccessibilityReport> RecentReports { get; set; } = new();

    // For Chart.js
    public string ShipmentStatusLabelsJson { get; set; } = "[]";
    public string ShipmentStatusDataJson { get; set; } = "[]";
    public string AccessibilityRatingLabelsJson { get; set; } = "[]";
    public string AccessibilityRatingDataJson { get; set; } = "[]";
}
