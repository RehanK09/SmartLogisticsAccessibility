# Project Report Notes — AI-Based Smart Logistics & Accessibility Intelligence Platform

**Project Code:** SIH26002  
**Course:** CSE253 (.NET Programming) — CA3 Allocated Project  
**Academic Year / Term:** 2026–2027  

---

## 1. Project Identity & Team Allocation

- **Official Project Title:** AI-Based Smart Logistics and Accessibility Intelligence Platform
- **Project Code:** SIH26002
- **Team Members:**
  1. **Peta Venkata Ashok** — Reg No: `RK3P25RAA28`
  2. **Rehan Khan** — Reg No: `RK3P25RAA29`
  3. **Ishaan Sehdev** — Reg No: `RK3P25RAA30`
- **GitHub Repository:** [https://github.com/RehanK09/SmartLogisticsAccessibility](https://github.com/RehanK09/SmartLogisticsAccessibility)

---

## 2. Problem Statement

Modern distribution and urban logistics networks often plan routes and delivery points purely based on linear distance or transit time, completely overlooking accessibility challenges. Warehouses, hubs, cargo terminals, and urban delivery destinations regularly feature physical barriers—such as missing wheelchair ramps, broken elevators, inaccessible loading docks, and absence of tactile paving. 

Consequently:
1. Deliveries to locations with severe accessibility impediments suffer from unexpected delays, damaged cargo, or delivery failures.
2. Logistics personnel with disabilities or mobility limitations face severe occupational barriers.
3. Urban administrators and supply chain planners lack a centralized, transparent intelligence platform that bridges operational logistics metrics with facility accessibility audits.

---

## 3. Introduction

The **AI-Based Smart Logistics and Accessibility Intelligence Platform (SIH26002)** is an enterprise web solution engineered in **C#**, **ASP.NET Core MVC (.NET 10)**, and **Entity Framework Core 10**, backed by **Microsoft SQL Server Express**. 

The platform merges operational supply chain logistics (shipment management, origin-destination routing, priority scheduling) with accessibility facility auditing (wheelchair accessibility, ramps, elevators, tactile guidance, audio signals). By computing an objective, rule-based accessibility rating for every facility and integrating accessibility weights into route comparison algorithms, the platform delivers an inclusive and efficient supply chain management environment.

---

## 4. Key Objectives

1. **Integrated Operational Logistics:** Deliver full CRUD (Create, Read, Update, Delete) capability for shipments, locations, and transportation routes with SQL Server persistence and data validation.
2. **Transparent Route Intelligence:** Provide an algorithmic, rule-based route recommendation engine that scores routes across speed, cost, accessibility level, and delivery priority without deceptive "black box" claims.
3. **Accessibility Audit & Scoring:** Enable structured accessibility reporting across 8 standardized accessibility features, computing an automated 0–100% accessibility score and visual classification (Fully Accessible, Partially Accessible, Has Issues).
4. **Geographic Network Visualization:** Render interactive geographic maps displaying facility markers colored by accessibility status using Leaflet and OpenStreetMap.

---

## 5. Technology Stack & Environment

| Layer | Component | Version / Specification |
| :--- | :--- | :--- |
| **Language** | C# | 14 / Modern C# |
| **Runtime & SDK** | .NET 10 | Target Framework: `net10.0` (SDK: 11.0.100) |
| **Web Framework** | ASP.NET Core MVC | Controller-Model-View architecture |
| **ORM** | Entity Framework Core | `10.0.15` (SqlServer, Tools, Design) |
| **Database** | Microsoft SQL Server Express | SQL Server 2025 Express (`QUANTUM\SQLEXPRESS`), Database: `RADB` |
| **Frontend Styling** | Bootstrap 5 & Bootstrap Icons | Responsive light SaaS UI layout |
| **Data Visualization** | Chart.js 4.4.3 | Interactive doughnut & bar charts |
| **Mapping Engine** | Leaflet 1.9.4 & OpenStreetMap | Zero-cost open-source spatial maps |
| **Development Environment**| Visual Studio Community 2026 | ASP.NET & web development workload (`NetWeb`) |

---

## 6. Architecture & Project Structure

The project strictly adheres to the standard **Model-View-Controller (MVC)** separation of concerns:

```text
SmartLogisticsAccessibility/
├── SmartLogisticsAccessibility.sln          # Visual Studio Solution File
├── SmartLogisticsAccessibility.slnx         # Modern XML Solution
└── SmartLogisticsAccessibility.Web/         # Primary ASP.NET Core MVC Project
    ├── Controllers/                         # Request handling & HTTP verbs
    │   ├── HomeController.cs                # Dashboard metrics, Chart.js feeds, About
    │   ├── ShipmentsController.cs           # Full CRUD for shipments
    │   ├── LocationsController.cs           # Full CRUD for logistic locations
    │   ├── RoutesController.cs              # Route management & intelligence query
    │   └── AccessibilityReportsController.cs# Accessibility report CRUD & Leaflet map
    ├── Data/                                # Persistence Layer
    │   ├── ApplicationDbContext.cs          # EF Core DbContext with model relations
    │   └── DbSeeder.cs                      # Sample data seeder (idempotent)
    ├── Migrations/                          # EF Core migration code
    │   └── 20261123130842_InitialCreate.cs  # SQL Server schema creation
    ├── Models/                              # Domain Entities
    │   ├── Shipment.cs                      # Shipment entity with status & priority enums
    │   ├── Location.cs                      # Facility entity with coordinates & type
    │   ├── Route.cs                         # Route entity with distance, time, cost & access
    │   └── AccessibilityReport.cs           # Audit report with score calculation logic
    ├── Services/                            # Business Logic
    │   └── RouteRecommendationService.cs    # 100-point rule-based recommendation engine
    ├── ViewModels/                          # Presentation Models
    │   └── DashboardViewModel.cs            # Aggregated statistics and chart serialization
    ├── Views/                               # Razor Views (.cshtml)
    │   ├── Home/ (Index.cshtml, About.cshtml)
    │   ├── Shipments/ (Index, Details, Create, Edit, Delete)
    │   ├── Locations/ (Index, Details, Create, Edit, Delete)
    │   ├── Routes/ (Index, Intelligence, Create, Edit, Delete)
    │   ├── AccessibilityReports/ (Index, Details, Create, Edit, Delete, Map)
    │   └── Shared/ (_Layout.cshtml, _ValidationScriptsPartial.cshtml)
    ├── wwwroot/                             # Client static assets (CSS, JS, Bootstrap)
    ├── appsettings.json                     # Database connection string
    └── Program.cs                           # DI registration, pipeline, and startup migrations
```

---

## 7. Database Design & SQL Server Schema

### Entities & Relationships
1. **`Locations` Table:**
   - Primary Key: `Id` (`int`, Identity)
   - Columns: `Name`, `Address`, `City`, `State`, `PostalCode`, `Latitude`, `Longitude`, `Type`, `IsActive`, `Notes`, `CreatedAt`, `IsSampleData`
   - Navigation: Has many `OriginShipments`, `DestinationShipments`, `AccessibilityReports`, `RoutesAsOrigin`, `RoutesAsDestination`.

2. **`Shipments` Table:**
   - Primary Key: `Id` (`int`, Identity)
   - Foreign Keys: `OriginLocationId` &rarr; `Locations(Id)`, `DestinationLocationId` &rarr; `Locations(Id)`, `AssignedRouteId` &rarr; `Routes(Id)` (nullable).
   - Constraints: Unique index on `TrackingNumber` (`nvarchar(30)`).
   - Columns: `Description`, `Status`, `Priority`, `SenderName`, `ReceiverName`, `WeightKg`, `EstimatedDeliveryDate`, `ActualDeliveryDate`, `Notes`, `CreatedAt`, `UpdatedAt`, `IsSampleData`.

3. **`Routes` Table:**
   - Primary Key: `Id` (`int`, Identity)
   - Foreign Keys: `OriginLocationId` &rarr; `Locations(Id)`, `DestinationLocationId` &rarr; `Locations(Id)`.
   - Columns: `Name`, `EstimatedDistanceKm`, `EstimatedTravelTimeHours`, `Type`, `AccessibilityLevel`, `EstimatedCostINR`, `IsActive`, `Notes`, `CreatedAt`, `IsSampleData`.

4. **`AccessibilityReports` Table:**
   - Primary Key: `Id` (`int`, Identity)
   - Foreign Key: `LocationId` &rarr; `Locations(Id)` (Cascade on delete).
   - Columns: `Title`, `ReporterName`, `WheelchairAccessible`, `RampsAvailable`, `ElevatorsAvailable`, `AccessibleEntrance`, `AccessibleParking`, `TactilePaving`, `AudioSignals`, `BrailleSignage`, `Obstacles`, `AdditionalNotes`, `Status`, `CreatedAt`, `UpdatedAt`, `IsSampleData`.

---

## 8. Implemented Functionalities & Working

### A. Dashboard & Analytics
- Live counters for total shipments, active in-transit, delivered, and delayed shipments.
- Chart.js visual breakdown of shipment status (doughnut) and facility accessibility distribution (bar chart).
- Quick tables for recent shipments and recent accessibility audits with direct navigation pills.

### B. Shipment Management (CRUD)
- **Create:** Generates tracking numbers, captures sender/receiver, weight, delivery dates, and routes. Validates required fields and enforces tracking number uniqueness.
- **Read:** Lists shipments with multi-field search (tracking number, description, sender, receiver) and dropdown filters by Status and Priority.
- **Details:** Comprehensive view with route estimates, sender/receiver info, and badges.
- **Update:** Pre-populates all existing data, allows state changes from `Pending` &rarr; `InTransit` &rarr; `Delivered`.
- **Delete:** Two-step confirmation screen preventing accidental data loss.

### C. Location Management (CRUD)
- Complete CRUD operations for warehouses, delivery hubs, ports, and airports.
- Coordinates (latitude and longitude) are validated and linked with interactive map pins.

### D. Route Intelligence & Transparent Scoring
- Implemented in `Services/RouteRecommendationService.cs`.
- Uses a **100-Point Weighted Scoring Algorithm**:
  $$\text{Score} = \text{Speed (40 pts)} + \text{Cost (30 pts)} + \text{Accessibility (20 pts)} + \text{Priority Bonus (10 pts)}$$
  * **Speed Score (40 pts):** Evaluated inversely against travel duration: $\text{Speed} = 40 \times (1 - \frac{T - T_{\min}}{T_{\max} - T_{\min}})$.
  * **Cost Score (30 pts):** Evaluated inversely against estimated cost: $\text{Cost} = 30 \times (1 - \frac{C - C_{\min}}{C_{\max} - C_{\min}})$.
  * **Accessibility Score (20 pts):** Full = 20 pts, Partial = 10 pts, Limited = 0 pts.
  * **Priority Bonus (10 pts):** If priority is High/Critical, awards up to 10 points for faster transit. If Low/Normal, awards up to 10 points for cost savings.
- Each route displays a detailed, human-readable breakdown explaining why it received its score and ranking.

### E. Accessibility Auditing & Map
- **8 Standardized Criteria:** Wheelchair Access, Ramps, Elevators, Accessible Entrances, Accessible Parking, Tactile Paving, Audio Signals, Braille Signage.
- **Automated Score:** Each verified feature contributes $12.5$ points: $\text{Score} = \frac{\text{Features Count}}{8} \times 100\%$.
- **Rating Categories:**
  - $\ge 75\%$: **Fully Accessible** (Green badge / marker)
  - $40\% - 74\%$: **Partially Accessible** (Amber badge / marker)
  - $< 40\%$: **Has Issues** (Red badge / marker)
- **Interactive Leaflet Map:** Displays facility pins on OpenStreetMap tiles. Clicking any pin displays the score, rating badge, address, and link to audit reports.

---

## 9. Sector Applications & Real-World Utility

| Sector | Practical Application & Value |
| :--- | :--- |
| **Logistics & Transportation** | Dispatchers plan freight corridors that avoid bottlenecks and select carrier routes equipped for barrier-free cargo movement. |
| **E-Commerce & Last-Mile Delivery** | Delivery riders receive advance alerts if an apartment complex or hub lacks ramps or working elevators, enabling appropriate staffing and parcel handling. |
| **Smart Cities & Urban Planning** | Municipal corporations identify accessibility deserts across commercial districts by filtering location audit scores on the platform map. |
| **Healthcare & Hospital Supply** | Critical vaccine and medical supply shipments are routed through highest-accessibility corridors to ensure zero delivery delays or handling obstructions. |
| **Public Transport & Intermodal Hubs** | Audits cargo transshipment between airports, rail terminals, and ports, pinpointing stations requiring accessibility infrastructure retrofitting. |
| **Warehousing & Supply Chain Operations** | Internal workplace safety teams maintain ADA / accessibility compliance for dockworkers and warehouse staff with disabilities. |

---

## 10. Advantages & System Limitations

### Advantages:
1. **Full Transparency:** No opaque AI hallucinations or hidden parameters; scoring logic is deterministic, reproducible, and explainable for academic and enterprise audits.
2. **Native .NET 10 & SQL Server Integration:** High-performance asynchronous database calls (`async`/`await`), strong entity typing, and robust migrations.
3. **Open Standards:** Built without costly proprietary APIs; uses Bootstrap 5, Chart.js, and OpenStreetMap/Leaflet.
4. **Resilient Seeding:** Pre-seeded with realistic, clearly tagged demo data that avoids duplication on re-runs.

### Current Limitations:
1. **Rule-Based Estimates:** Distances and costs are currently estimated static inputs rather than dynamic live GPS telemetry feeds.
2. **Single-Tenant Local Deployment:** Designed for local SQL Server Express execution under Windows Authentication; multi-tenant cloud authentication (e.g. Azure AD) is not yet integrated.

---

## 11. Future Scope

1. **Machine Learning Route Optimization:** Integrate `ML.NET` to predict delivery delay probabilities based on historical weather, traffic, and accessibility bottlenecks.
2. **Crowdsourced Photo Audits:** Allow field delivery personnel to upload geotagged smartphone photos of accessibility obstacles directly into report records.
3. **Automated Live GPS Telemetry:** Interface with IoT vehicle trackers to stream real-time latitude/longitude coordinates to the logistics dashboard.

---

## 12. Seven Screenshots Mapping for Final Academic Report

| # | Screenshot Filename | Section / Purpose | Description |
| :-: | :--- | :--- | :--- |
| **1** | `01_Dashboard.png` | **Section 7 (Platform Overview)** | Executive dashboard showing shipment statistics, Chart.js graphs, and recent activity tables. |
| **2** | `02_Shipment_Management.png` | **Section 7 (Logistics Module)** | Shipment records table featuring status badges, priority indicators, search, and action buttons. |
| **3** | `03_Shipment_Form.png` | **Section 7 (CRUD Operations)** | Validated shipment creation/editing form with origin, destination, weight, and status inputs. |
| **4** | `04_Route_Intelligence.png` | **Section 7 (Route Engine)** | Route recommendation engine comparing routes with composite scores and factor breakdowns. |
| **5** | `05_Accessibility_Map.png` | **Section 8 (Accessibility Module)** | Leaflet & OpenStreetMap view showing color-coded location markers based on accessibility scores. |
| **6** | `06_Accessibility_Reports.png` | **Section 8 (Audit Module)** | Audit reports list displaying 8-criteria scores, ratings, and obstacle notes. |
| **7** | `07_Development_Environment.png` | **Section 8 (Environment Verification)**| Development environment showing Visual Studio solution structure, C# project files, and SQL Server setup. |

---

## 13. References & Official Documentation

1. Microsoft Learn: *ASP.NET Core MVC Overview* — [https://learn.microsoft.com/aspnet/core/mvc/overview](https://learn.microsoft.com/aspnet/core/mvc/overview)
2. Microsoft Learn: *Entity Framework Core Documentation* — [https://learn.microsoft.com/ef/core/](https://learn.microsoft.com/ef/core/)
3. Leaflet JavaScript Library: *An Open-Source JavaScript Library for Mobile-Friendly Interactive Maps* — [https://leafletjs.com/](https://leafletjs.com/)
4. Chart.js Documentation: *Simple yet flexible JavaScript charting for designers & developers* — [https://www.chartjs.org/docs/latest/](https://www.chartjs.org/docs/latest/)
5. OpenStreetMap Foundation: *OpenStreetMap Copyright and License* — [https://www.openstreetmap.org/copyright](https://www.openstreetmap.org/copyright)
