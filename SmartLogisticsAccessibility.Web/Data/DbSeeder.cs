using SmartLogisticsAccessibility.Web.Models;
using Microsoft.EntityFrameworkCore;
using LogisticsRoute = SmartLogisticsAccessibility.Web.Models.Route;

namespace SmartLogisticsAccessibility.Web.Data;

/// <summary>
/// Seeds realistic sample records into the database on first startup.
/// All seeded records are flagged with IsSampleData = true so they can be
/// identified and removed easily.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Only seed if the database is empty
        if (await context.Locations.AnyAsync())
            return;

        // ---- Locations ---------------------------------------------------------
        var locations = new List<Location>
        {
            new() { Name = "LPU Main Warehouse", Address = "Lovely Professional University Campus", City = "Phagwara", State = "Punjab", PostalCode = "144411", Latitude = 31.2545, Longitude = 75.7042, Type = LocationType.Warehouse, IsActive = true, IsSampleData = true, Notes = "Primary distribution centre for northern region (SAMPLE DATA)" },
            new() { Name = "Delhi Hub", Address = "Nehru Place Industrial Area", City = "New Delhi", State = "Delhi", PostalCode = "110019", Latitude = 28.5484, Longitude = 77.2510, Type = LocationType.Hub, IsActive = true, IsSampleData = true, Notes = "Central hub for NCR deliveries (SAMPLE DATA)" },
            new() { Name = "Mumbai Port Terminal", Address = "Nhava Sheva, JNPT", City = "Navi Mumbai", State = "Maharashtra", PostalCode = "400707", Latitude = 18.9450, Longitude = 72.9502, Type = LocationType.Port, IsActive = true, IsSampleData = true, Notes = "International freight entry point (SAMPLE DATA)" },
            new() { Name = "Bangalore Tech Park Hub", Address = "Electronics City Phase 1", City = "Bengaluru", State = "Karnataka", PostalCode = "560100", Latitude = 12.8456, Longitude = 77.6600, Type = LocationType.Hub, IsActive = true, IsSampleData = true, Notes = "South India distribution hub (SAMPLE DATA)" },
            new() { Name = "Chennai Airport Cargo", Address = "Meenambakkam Airport Road", City = "Chennai", State = "Tamil Nadu", PostalCode = "600027", Latitude = 12.9941, Longitude = 80.1709, Type = LocationType.Airport, IsActive = true, IsSampleData = true, Notes = "Air cargo terminal (SAMPLE DATA)" },
            new() { Name = "Hyderabad Delivery Point", Address = "HITEC City, Madhapur", City = "Hyderabad", State = "Telangana", PostalCode = "500081", Latitude = 17.4474, Longitude = 78.3762, Type = LocationType.DeliveryPoint, IsActive = true, IsSampleData = true, Notes = "Last-mile delivery point (SAMPLE DATA)" },
            new() { Name = "Kolkata East Warehouse", Address = "Saltlake Sector V", City = "Kolkata", State = "West Bengal", PostalCode = "700091", Latitude = 22.5726, Longitude = 88.4312, Type = LocationType.Warehouse, IsActive = true, IsSampleData = true, Notes = "East India distribution (SAMPLE DATA)" },
        };
        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();

        var lpu = locations[0];
        var delhi = locations[1];
        var mumbai = locations[2];
        var bangalore = locations[3];
        var chennai = locations[4];
        var hyderabad = locations[5];
        var kolkata = locations[6];

        // ---- Routes ------------------------------------------------------------
        var routes = new List<LogisticsRoute>
        {
            new() { Name = "LPU → Delhi (NH1 Expressway)", OriginLocationId = lpu.Id, DestinationLocationId = delhi.Id, EstimatedDistanceKm = 345, EstimatedTravelTimeHours = 5.5, Type = RouteType.Road, AccessibilityLevel = AccessibilityLevel.Full, EstimatedCostINR = 4800, IsActive = true, IsSampleData = true, Notes = "Via NH1; toll-based highway (SAMPLE ESTIMATE)" },
            new() { Name = "LPU → Delhi (State Highway — Alt)", OriginLocationId = lpu.Id, DestinationLocationId = delhi.Id, EstimatedDistanceKm = 380, EstimatedTravelTimeHours = 7.0, Type = RouteType.Road, AccessibilityLevel = AccessibilityLevel.Partial, EstimatedCostINR = 3200, IsActive = true, IsSampleData = true, Notes = "Via state highway; cheaper but longer (SAMPLE ESTIMATE)" },
            new() { Name = "Delhi → Mumbai (Rail Freight)", OriginLocationId = delhi.Id, DestinationLocationId = mumbai.Id, EstimatedDistanceKm = 1420, EstimatedTravelTimeHours = 22, Type = RouteType.Rail, AccessibilityLevel = AccessibilityLevel.Partial, EstimatedCostINR = 18000, IsActive = true, IsSampleData = true, Notes = "Indian Railways freight (SAMPLE ESTIMATE)" },
            new() { Name = "Delhi → Mumbai (Air Cargo)", OriginLocationId = delhi.Id, DestinationLocationId = mumbai.Id, EstimatedDistanceKm = 1148, EstimatedTravelTimeHours = 2.5, Type = RouteType.Air, AccessibilityLevel = AccessibilityLevel.Full, EstimatedCostINR = 65000, IsActive = true, IsSampleData = true, Notes = "Express air cargo (SAMPLE ESTIMATE)" },
            new() { Name = "Mumbai → Bangalore (Road)", OriginLocationId = mumbai.Id, DestinationLocationId = bangalore.Id, EstimatedDistanceKm = 981, EstimatedTravelTimeHours = 15, Type = RouteType.Road, AccessibilityLevel = AccessibilityLevel.Partial, EstimatedCostINR = 14500, IsActive = true, IsSampleData = true, Notes = "Via NH48 (SAMPLE ESTIMATE)" },
            new() { Name = "Bangalore → Chennai (Road)", OriginLocationId = bangalore.Id, DestinationLocationId = chennai.Id, EstimatedDistanceKm = 346, EstimatedTravelTimeHours = 5.0, Type = RouteType.Road, AccessibilityLevel = AccessibilityLevel.Full, EstimatedCostINR = 5200, IsActive = true, IsSampleData = true, Notes = "Via NH48 (SAMPLE ESTIMATE)" },
            new() { Name = "Delhi → Kolkata (Rail)", OriginLocationId = delhi.Id, DestinationLocationId = kolkata.Id, EstimatedDistanceKm = 1472, EstimatedTravelTimeHours = 18, Type = RouteType.Rail, AccessibilityLevel = AccessibilityLevel.Partial, EstimatedCostINR = 16000, IsActive = true, IsSampleData = true, Notes = "Freight rail (SAMPLE ESTIMATE)" },
        };
        context.Routes.AddRange(routes);
        await context.SaveChangesAsync();

        // ---- Shipments ---------------------------------------------------------
        var shipments = new List<Shipment>
        {
            new() { TrackingNumber = "SL-2026-0001", Description = "Industrial machinery components", OriginLocationId = lpu.Id, DestinationLocationId = delhi.Id, Status = ShipmentStatus.InTransit, Priority = ShipmentPriority.High, SenderName = "LPU Engineering Dept.", ReceiverName = "Delhi Manufacturing Co.", WeightKg = 850, EstimatedDeliveryDate = DateTime.Today.AddDays(2), AssignedRouteId = routes[0].Id, IsSampleData = true, Notes = "SAMPLE DATA" },
            new() { TrackingNumber = "SL-2026-0002", Description = "Medical supplies — temperature sensitive", OriginLocationId = delhi.Id, DestinationLocationId = mumbai.Id, Status = ShipmentStatus.InTransit, Priority = ShipmentPriority.Critical, SenderName = "Apollo Healthcare", ReceiverName = "Mumbai City Hospital", WeightKg = 120, EstimatedDeliveryDate = DateTime.Today.AddDays(1), AssignedRouteId = routes[3].Id, IsSampleData = true, Notes = "SAMPLE DATA — requires cold chain" },
            new() { TrackingNumber = "SL-2026-0003", Description = "Electronic components — bulk", OriginLocationId = mumbai.Id, DestinationLocationId = bangalore.Id, Status = ShipmentStatus.Delivered, Priority = ShipmentPriority.Normal, SenderName = "Reliance Tech", ReceiverName = "Infosys IT Park", WeightKg = 320, EstimatedDeliveryDate = DateTime.Today.AddDays(-3), ActualDeliveryDate = DateTime.Today.AddDays(-3), AssignedRouteId = routes[4].Id, IsSampleData = true, Notes = "SAMPLE DATA" },
            new() { TrackingNumber = "SL-2026-0004", Description = "Automotive spare parts", OriginLocationId = bangalore.Id, DestinationLocationId = chennai.Id, Status = ShipmentStatus.Pending, Priority = ShipmentPriority.Normal, SenderName = "Toyota India", ReceiverName = "Chennai Auto Dealer", WeightKg = 560, EstimatedDeliveryDate = DateTime.Today.AddDays(5), IsSampleData = true, Notes = "SAMPLE DATA" },
            new() { TrackingNumber = "SL-2026-0005", Description = "Textile goods — wholesale", OriginLocationId = delhi.Id, DestinationLocationId = kolkata.Id, Status = ShipmentStatus.Delayed, Priority = ShipmentPriority.Low, SenderName = "Delhi Fabrics Ltd.", ReceiverName = "Kolkata Retail Hub", WeightKg = 1200, EstimatedDeliveryDate = DateTime.Today.AddDays(-1), AssignedRouteId = routes[6].Id, IsSampleData = true, Notes = "SAMPLE DATA — delayed due to weather" },
            new() { TrackingNumber = "SL-2026-0006", Description = "Laboratory equipment", OriginLocationId = chennai.Id, DestinationLocationId = hyderabad.Id, Status = ShipmentStatus.Pending, Priority = ShipmentPriority.High, SenderName = "IIT Madras Research", ReceiverName = "DRDO Hyderabad", WeightKg = 95, EstimatedDeliveryDate = DateTime.Today.AddDays(3), IsSampleData = true, Notes = "SAMPLE DATA" },
            new() { TrackingNumber = "SL-2026-0007", Description = "Food products — perishable", OriginLocationId = lpu.Id, DestinationLocationId = delhi.Id, Status = ShipmentStatus.Cancelled, Priority = ShipmentPriority.High, SenderName = "Punjab Agro Corp.", ReceiverName = "Delhi Supermart", WeightKg = 2000, IsSampleData = true, Notes = "SAMPLE DATA — cancelled due to quality check failure" },
        };
        context.Shipments.AddRange(shipments);
        await context.SaveChangesAsync();

        // ---- Accessibility Reports ---------------------------------------------
        var reports = new List<AccessibilityReport>
        {
            new() { LocationId = lpu.Id, Title = "LPU Warehouse — Accessibility Audit", ReporterName = "Peta Venkata Ashok", WheelchairAccessible = true, RampsAvailable = true, ElevatorsAvailable = true, AccessibleEntrance = true, AccessibleParking = true, TactilePaving = false, AudioSignals = false, BrailleSignage = false, Status = ReportStatus.Verified, IsSampleData = true, AdditionalNotes = "SAMPLE DATA — Main warehouse has good basic accessibility" },
            new() { LocationId = delhi.Id, Title = "Delhi Hub — Full Accessibility Check", ReporterName = "Rehan Khan", WheelchairAccessible = true, RampsAvailable = true, ElevatorsAvailable = true, AccessibleEntrance = true, AccessibleParking = true, TactilePaving = true, AudioSignals = true, BrailleSignage = true, Status = ReportStatus.Verified, IsSampleData = true, AdditionalNotes = "SAMPLE DATA — Highest rated hub in network" },
            new() { LocationId = mumbai.Id, Title = "Mumbai Port — Accessibility Report", ReporterName = "Ishaan Sehdev", WheelchairAccessible = true, RampsAvailable = true, ElevatorsAvailable = false, AccessibleEntrance = true, AccessibleParking = false, TactilePaving = false, AudioSignals = false, BrailleSignage = false, Status = ReportStatus.Submitted, IsSampleData = true, Obstacles = "Uneven port surface in cargo loading zones (SAMPLE DATA)" },
            new() { LocationId = bangalore.Id, Title = "Bangalore Tech Hub — Accessibility Review", ReporterName = "Peta Venkata Ashok", WheelchairAccessible = true, RampsAvailable = false, ElevatorsAvailable = true, AccessibleEntrance = false, AccessibleParking = true, TactilePaving = false, AudioSignals = false, BrailleSignage = false, Status = ReportStatus.Submitted, IsSampleData = true, Obstacles = "Ramp construction pending at south entrance (SAMPLE DATA)" },
            new() { LocationId = chennai.Id, Title = "Chennai Airport Cargo — Accessibility Assessment", ReporterName = "Rehan Khan", WheelchairAccessible = true, RampsAvailable = true, ElevatorsAvailable = true, AccessibleEntrance = true, AccessibleParking = true, TactilePaving = true, AudioSignals = false, BrailleSignage = false, Status = ReportStatus.Verified, IsSampleData = true, AdditionalNotes = "SAMPLE DATA — Airport standard accessibility" },
            new() { LocationId = kolkata.Id, Title = "Kolkata Warehouse — Accessibility Issues Found", ReporterName = "Ishaan Sehdev", WheelchairAccessible = false, RampsAvailable = false, ElevatorsAvailable = false, AccessibleEntrance = false, AccessibleParking = false, TactilePaving = false, AudioSignals = false, BrailleSignage = false, Status = ReportStatus.Submitted, IsSampleData = true, Obstacles = "No accessible entrance, no ramps, steep loading dock (SAMPLE DATA)", AdditionalNotes = "SAMPLE DATA — Requires major accessibility improvements" },
        };
        context.AccessibilityReports.AddRange(reports);
        await context.SaveChangesAsync();
    }
}
