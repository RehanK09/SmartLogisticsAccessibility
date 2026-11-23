using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLogisticsAccessibility.Web.Models;

/// <summary>
/// An accessibility report for a specific location, submitted by a user or created as sample data.
/// The accessibility score is calculated from the boolean accessibility features.
/// </summary>
public class AccessibilityReport
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Location")]
    public int LocationId { get; set; }

    [Required, StringLength(150)]
    [Display(Name = "Report Title")]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(120)]
    [Display(Name = "Reporter Name")]
    public string ReporterName { get; set; } = string.Empty;

    // ---- Accessibility Features ------------------------------------------------
    [Display(Name = "Wheelchair Accessible")]
    public bool WheelchairAccessible { get; set; }

    [Display(Name = "Ramps Available")]
    public bool RampsAvailable { get; set; }

    [Display(Name = "Elevators Available")]
    public bool ElevatorsAvailable { get; set; }

    [Display(Name = "Accessible Entrance")]
    public bool AccessibleEntrance { get; set; }

    [Display(Name = "Accessible Parking")]
    public bool AccessibleParking { get; set; }

    [Display(Name = "Tactile Paving")]
    public bool TactilePaving { get; set; }

    [Display(Name = "Audio Signals")]
    public bool AudioSignals { get; set; }

    [Display(Name = "Braille Signage")]
    public bool BrailleSignage { get; set; }

    // ---- Obstacles and Issues --------------------------------------------------
    [StringLength(600)]
    [Display(Name = "Identified Obstacles")]
    public string? Obstacles { get; set; }

    [StringLength(600)]
    [Display(Name = "Additional Notes")]
    public string? AdditionalNotes { get; set; }

    // ---- Status ----------------------------------------------------------------
    [Display(Name = "Report Status")]
    public ReportStatus Status { get; set; } = ReportStatus.Submitted;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsSampleData { get; set; } = false;

    // Navigation
    public Location? Location { get; set; }

    // ---- Computed Properties ---------------------------------------------------

    /// <summary>
    /// Calculates the accessibility score (0-100) based on the 8 boolean feature fields.
    /// Each feature contributes 12.5 points (8 × 12.5 = 100).
    /// This is a simple, transparent rule-based score — not an AI prediction.
    /// </summary>
    [NotMapped]
    public int AccessibilityScore
    {
        get
        {
            int count = 0;
            if (WheelchairAccessible) count++;
            if (RampsAvailable) count++;
            if (ElevatorsAvailable) count++;
            if (AccessibleEntrance) count++;
            if (AccessibleParking) count++;
            if (TactilePaving) count++;
            if (AudioSignals) count++;
            if (BrailleSignage) count++;
            return (int)Math.Round(count / 8.0 * 100);
        }
    }

    /// <summary>Derived accessibility rating based on the score.</summary>
    [NotMapped]
    public AccessibilityRating Rating => AccessibilityScore >= 75
        ? AccessibilityRating.FullyAccessible
        : AccessibilityScore >= 40
            ? AccessibilityRating.PartiallyAccessible
            : AccessibilityRating.HasIssues;

    [NotMapped]
    public string RatingBadgeClass => Rating switch
    {
        AccessibilityRating.FullyAccessible => "badge-success",
        AccessibilityRating.PartiallyAccessible => "badge-warning",
        AccessibilityRating.HasIssues => "badge-danger",
        _ => "badge-secondary"
    };

    [NotMapped]
    public string RatingLabel => Rating switch
    {
        AccessibilityRating.FullyAccessible => "Fully Accessible",
        AccessibilityRating.PartiallyAccessible => "Partially Accessible",
        AccessibilityRating.HasIssues => "Has Issues",
        _ => "Unknown"
    };
}

public enum AccessibilityRating
{
    [Display(Name = "Fully Accessible")]
    FullyAccessible,
    [Display(Name = "Partially Accessible")]
    PartiallyAccessible,
    [Display(Name = "Has Issues")]
    HasIssues
}

public enum ReportStatus
{
    Submitted,
    Verified,
    Disputed
}
