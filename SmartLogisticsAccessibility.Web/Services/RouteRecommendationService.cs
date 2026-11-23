using SmartLogisticsAccessibility.Web.Models;
using LogisticsRoute = SmartLogisticsAccessibility.Web.Models.Route;

namespace SmartLogisticsAccessibility.Web.Services;

/// <summary>
/// View model for a single route recommendation result.
/// </summary>
public class RouteRecommendation
{
    public LogisticsRoute Route { get; set; } = null!;

    /// <summary>Composite recommendation score (0–100). Higher is better.</summary>
    public double Score { get; set; }

    /// <summary>Rank among compared routes (1 = best).</summary>
    public int Rank { get; set; }

    public bool IsRecommended => Rank == 1;

    /// <summary>Human-readable explanation of how the score was calculated.</summary>
    public string Explanation { get; set; } = string.Empty;

    // Sub-scores (each 0–100, used to build the composite)
    public double SpeedScore { get; set; }
    public double CostScore { get; set; }
    public double AccessibilityScore { get; set; }
    public double PriorityBonus { get; set; }
}

/// <summary>
/// Rule-based Route Recommendation Service for the Smart Logistics platform.
///
/// How the score works (all values are estimates — clearly labelled):
///   • Speed score     (40 pts): Lower travel time → higher speed score.
///   • Cost score      (30 pts): Lower cost → higher cost score.
///   • Accessibility   (20 pts): Full = 20, Partial = 10, Limited = 0.
///   • Priority bonus  (10 pts): For High/Critical shipments, faster routes
///                               get a bonus to prefer speed over cost.
///
/// No machine-learning model is used. This is a transparent weighted scoring
/// algorithm implemented in C#. It is designed so that a more sophisticated
/// algorithm can be substituted later by replacing this service.
/// </summary>
public class RouteRecommendationService
{
    // Weight constants — must add to 100
    private const double SpeedWeight = 40.0;
    private const double CostWeight = 30.0;
    private const double AccessibilityWeight = 20.0;
    private const double PriorityBonusWeight = 10.0;

    /// <summary>
    /// Scores and ranks a list of route options for a given shipment priority.
    /// Returns the same list sorted best-first with scores and explanations attached.
    /// </summary>
    public List<RouteRecommendation> Recommend(
        IList<LogisticsRoute> routes,
        ShipmentPriority priority = ShipmentPriority.Normal)
    {
        if (routes == null || routes.Count == 0)
            return new List<RouteRecommendation>();

        // Collect normalisation bounds
        double minTime = routes.Min(r => r.EstimatedTravelTimeHours);
        double maxTime = routes.Max(r => r.EstimatedTravelTimeHours);
        double minCost = (double)(routes.Min(r => r.EstimatedCostINR ?? 0));
        double maxCost = (double)(routes.Max(r => r.EstimatedCostINR ?? 0));

        bool highPriority = priority is ShipmentPriority.High or ShipmentPriority.Critical;

        var results = new List<RouteRecommendation>();

        foreach (LogisticsRoute route in routes)
        {
            // ---- Speed score (lower time = better) ----------------------------
            double speedScore = maxTime == minTime
                ? SpeedWeight   // all routes have same travel time → everyone gets full speed score
                : SpeedWeight * (1.0 - (route.EstimatedTravelTimeHours - minTime) / (maxTime - minTime));

            // ---- Cost score (lower cost = better) -----------------------------
            double routeCost = (double)(route.EstimatedCostINR ?? 0);
            double costScore = maxCost == minCost
                ? CostWeight
                : CostWeight * (1.0 - (routeCost - minCost) / (maxCost - minCost));

            // ---- Accessibility score ------------------------------------------
            double accessScore = route.AccessibilityLevel switch
            {
                AccessibilityLevel.Full    => AccessibilityWeight,
                AccessibilityLevel.Partial => AccessibilityWeight * 0.5,
                AccessibilityLevel.Limited => 0,
                _                          => 0
            };

            // ---- Priority bonus (high/critical shipments prefer speed) --------
            double priorityBonus = 0;
            if (highPriority)
            {
                // Award extra points proportional to how fast this route is
                priorityBonus = maxTime == minTime
                    ? PriorityBonusWeight
                    : PriorityBonusWeight * (1.0 - (route.EstimatedTravelTimeHours - minTime) / (maxTime - minTime));
            }
            else
            {
                // For low/normal priority, the bonus goes to cost savings instead
                priorityBonus = maxCost == minCost
                    ? PriorityBonusWeight
                    : PriorityBonusWeight * (1.0 - (routeCost - minCost) / (maxCost - minCost));
            }

            double total = speedScore + costScore + accessScore + priorityBonus;

            // ---- Build explanation text ---------------------------------------
            string explanation = BuildExplanation(route, speedScore, costScore, accessScore, priorityBonus, priority, highPriority);

            results.Add(new RouteRecommendation
            {
                Route = route,
                Score = Math.Round(total, 1),
                SpeedScore = Math.Round(speedScore, 1),
                CostScore = Math.Round(costScore, 1),
                AccessibilityScore = Math.Round(accessScore, 1),
                PriorityBonus = Math.Round(priorityBonus, 1),
                Explanation = explanation
            });
        }

        // Sort descending by score, then assign ranks
        results = results.OrderByDescending(r => r.Score).ToList();
        for (int i = 0; i < results.Count; i++)
            results[i].Rank = i + 1;

        return results;
    }

    private static string BuildExplanation(
        LogisticsRoute route, double speed, double cost, double access, double bonus,
        ShipmentPriority priority, bool highPriority)
    {
        var parts = new List<string>
        {
            $"Speed: {speed:F1}/40 pts — {route.EstimatedDistanceKm} km, ~{route.EstimatedTravelTimeHours:F1} hrs (est.)",
            $"Cost: {cost:F1}/30 pts — ₹{route.EstimatedCostINR:N0} (est.)",
            $"Accessibility: {access:F1}/20 pts — {route.AccessibilityLevel} level",
        };

        if (highPriority)
            parts.Add($"Priority bonus: {bonus:F1}/10 pts — High/Critical shipments reward faster routes.");
        else
            parts.Add($"Priority bonus: {bonus:F1}/10 pts — Low/Normal priority rewards cost efficiency.");

        return string.Join(" | ", parts);
    }
}
