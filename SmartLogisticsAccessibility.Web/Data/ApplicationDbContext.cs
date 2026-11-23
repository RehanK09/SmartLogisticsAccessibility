using Microsoft.EntityFrameworkCore;
using SmartLogisticsAccessibility.Web.Models;
using LogisticsRoute = SmartLogisticsAccessibility.Web.Models.Route;

namespace SmartLogisticsAccessibility.Web.Data;

/// <summary>
/// The main Entity Framework Core database context for the Smart Logistics &amp; Accessibility platform.
/// All database tables (DbSet properties) are defined here.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ---- DbSet properties define the SQL Server tables -------------------------
    public DbSet<Location> Locations { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<LogisticsRoute> Routes { get; set; }
    public DbSet<AccessibilityReport> AccessibilityReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Location ----------------------------------------------------------
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasIndex(l => l.Name);
            entity.Property(l => l.Type)
                  .HasConversion<string>();
        });

        // ---- Shipment ----------------------------------------------------------
        modelBuilder.Entity<Shipment>(entity =>
        {
            // A tracking number must be unique across the system
            entity.HasIndex(s => s.TrackingNumber).IsUnique();

            entity.Property(s => s.Status).HasConversion<string>();
            entity.Property(s => s.Priority).HasConversion<string>();

            // Shipment → OriginLocation (restrict delete so we cannot delete a
            // location that still has outbound shipments)
            entity.HasOne(s => s.OriginLocation)
                  .WithMany(l => l.OriginShipments)
                  .HasForeignKey(s => s.OriginLocationId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Shipment → DestinationLocation
            entity.HasOne(s => s.DestinationLocation)
                  .WithMany(l => l.DestinationShipments)
                  .HasForeignKey(s => s.DestinationLocationId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Shipment → Route (optional)
            entity.HasOne(s => s.AssignedRoute)
                  .WithMany(r => r.Shipments)
                  .HasForeignKey(s => s.AssignedRouteId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ---- Route -------------------------------------------------------------
        modelBuilder.Entity<LogisticsRoute>(entity =>
        {
            entity.Property(r => r.Type).HasConversion<string>();
            entity.Property(r => r.AccessibilityLevel).HasConversion<string>();

            entity.HasOne(r => r.OriginLocation)
                  .WithMany(l => l.RoutesAsOrigin)
                  .HasForeignKey(r => r.OriginLocationId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.DestinationLocation)
                  .WithMany(l => l.RoutesAsDestination)
                  .HasForeignKey(r => r.DestinationLocationId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- AccessibilityReport -----------------------------------------------
        modelBuilder.Entity<AccessibilityReport>(entity =>
        {
            entity.Property(a => a.Status).HasConversion<string>();

            entity.HasOne(a => a.Location)
                  .WithMany(l => l.AccessibilityReports)
                  .HasForeignKey(a => a.LocationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
