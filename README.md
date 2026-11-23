# AI-Based Smart Logistics and Accessibility Intelligence Platform

**Course:** CSE253 (.NET Programming) — CA3 Allocated Project  
**Project Code:** SIH26002  
**Repository:** [https://github.com/RehanK09/SmartLogisticsAccessibility](https://github.com/RehanK09/SmartLogisticsAccessibility)

---

## 👥 Team Members

| Name | Registration Number | Role |
| :--- | :---: | :--- |
| **Peta Venkata Ashok** | `RK3P25RAA28` | Full-Stack & Logistics Module |
| **Rehan Khan** | `RK3P25RAA29` | Full-Stack, Architecture & Database |
| **Ishaan Sehdev** | `RK3P25RAA30` | Accessibility Module & UI Design |

---

## 🛠️ Technology Stack & Architecture

- **Language:** C# 14
- **Web Framework:** ASP.NET Core MVC (.NET 10 / `net10.0`)
- **ORM:** Entity Framework Core `10.0.15` (SQL Server provider)
- **Database:** Microsoft SQL Server 2025 Express (`QUANTUM\SQLEXPRESS`)
- **Database Name:** `RADB`
- **UI Framework:** Bootstrap 5, Bootstrap Icons
- **Data Visualization:** Chart.js 4.4.3
- **Geographic Mapping:** Leaflet 1.9.4 & OpenStreetMap
- **IDE:** Visual Studio Community 2026

---

## 🌟 Key Functional Features

### 1. Smart Logistics Intelligence
- **Shipment Management:** Full CRUD operations for shipments with automatic tracking number generation, multi-field search, status filtering, and priority indicators.
- **Location Management:** Track warehouses, hubs, ports, and delivery points with coordinates and full address details.
- **Route Intelligence:** Transparent 100-point rule-based recommendation algorithm comparing routes based on **Speed (40%)**, **Cost (30%)**, **Accessibility (20%)**, and **Priority Bonus (10%)**. Shows exact scoring breakdown with plain-English explanations.

### 2. Accessibility Intelligence
- **8-Criteria Facility Auditing:** Wheelchair access, ramps, elevators, accessible entrances, accessible parking, tactile paving, audio signals, and braille signage.
- **Automated Scoring:** Calculates transparent 0–100% score (12.5 pts per feature) and categorizes facilities into *Fully Accessible* ($\ge 75\%$), *Partially Accessible* ($40–74\%$), or *Has Issues* ($< 40\%$).
- **Interactive Accessibility Map:** Interactive Leaflet map displaying facility markers color-coded by accessibility rating (Green / Amber / Red) with popup audit summaries.

### 3. Executive Dashboard
- Live counters for shipments, in-transit parcels, delivery status, and accessibility compliance.
- Interactive Chart.js charts (Shipment Status Doughnut & Accessibility Distribution Bar Chart).
- Recent activity tables with one-click navigation.

---

## 🚀 Quick Start Guide

### Prerequisites
1. [.NET 10 SDK](https://dotnet.microsoft.com/) (Version 11.0.100 or later with net10.0 support)
2. [Visual Studio Community 2026](https://visualstudio.microsoft.com/) with **ASP.NET and web development** workload
3. [Microsoft SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) & [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### How to Open & Run in Visual Studio Community
1. Launch **Visual Studio Community 2026**.
2. Select **Open a project or solution**.
3. Browse to and open:
   ```text
   SmartLogisticsAccessibility.sln
   ```
4. Verify your connection string in `SmartLogisticsAccessibility.Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=QUANTUM\\SQLEXPRESS;Database=RADB;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```
   *(Replace `QUANTUM\SQLEXPRESS` with your SQL Server instance name if different, e.g. `localhost\SQLEXPRESS` or `.\SQLEXPRESS`).*
5. Right-click the `SmartLogisticsAccessibility.Web` project &rarr; **Set as Startup Project**.
6. Press <kbd>F5</kbd> (or <kbd>Ctrl</kbd>+<kbd>F5</kbd>) to compile and run. The database is automatically migrated and seeded with sample data on first run!

### Running via Terminal / CLI
```powershell
# 1. Clone repository
git clone https://github.com/RehanK09/SmartLogisticsAccessibility.git
cd SmartLogisticsAccessibility

# 2. Restore NuGet packages and build
dotnet restore
dotnet build

# 3. Apply database migrations to SQL Server
dotnet ef database update --project SmartLogisticsAccessibility.Web

# 4. Run application
dotnet run --project SmartLogisticsAccessibility.Web
```
Visit `http://localhost:5000` (or the URL printed in the console).

---

## 🎓 Viva & Faculty Demonstration Guide

### How a Request Flows Through the Application:
$$\text{Browser (HTTP Request)} \longrightarrow \text{Controller} \longrightarrow \text{DbContext (EF Core)} \longleftrightarrow \text{SQL Server (RADB)} \longrightarrow \text{View (Razor HTML)}$$

1. **Routing:** When you click a link like `/Shipments`, the ASP.NET Core routing engine maps the URL to `ShipmentsController.cs` and invokes the `Index()` action method.
2. **Controller Logic:** The controller calls `_db.Shipments.Include(...).ToListAsync()` asynchronously to query the database.
3. **ORM (EF Core):** Entity Framework Core translates the C# LINQ query into an optimized SQL `SELECT` statement and executes it on `QUANTUM\SQLEXPRESS`.
4. **Data Hydration:** SQL Server returns the rows, and EF Core creates strongly-typed `Shipment` C# model instances.
5. **View Rendering:** The controller passes the model list to `Views/Shipments/Index.cshtml`. The Razor engine renders the HTML, CSS classes, and badges on the server and delivers the page to your browser.

### Key Files to Review Before Your Viva:
- **`Program.cs`:** Shows dependency injection (`AddDbContext`, `AddSingleton<RouteRecommendationService>`), middleware pipeline, and automatic database migration/seeding on startup.
- **`Data/ApplicationDbContext.cs`:** Defines the database tables (`DbSet<Shipment>`, `DbSet<Location>`, etc.) and entity relationships configured via Fluent API.
- **`Models/AccessibilityReport.cs`:** Contains the automated accessibility score calculation property (`AccessibilityScore`) and rating classification.
- **`Services/RouteRecommendationService.cs`:** Houses the 100-point rule-based recommendation logic and mathematical scoring breakdown.
- **`Controllers/ShipmentsController.cs`:** Demonstrates clean asynchronous CRUD methods (`Create`, `Edit`, `Delete`, `Details`) with anti-forgery token protection and model validation.
- **`Views/AccessibilityReports/Map.cshtml`:** Illustrates Leaflet and OpenStreetMap integration rendering location pins with customized accessibility popups.

---

## 📁 Repository Directory Structure

```text
SmartLogisticsAccessibility/
├── SmartLogisticsAccessibility.sln         # Solution file
├── SmartLogisticsAccessibility.slnx        # Modern solution format
├── README.md                               # Project documentation
├── Report/                                 # Academic project evidence
│   ├── Documentation/
│   │   └── Project_Report_Notes.md         # Comprehensive academic report notes
│   └── Screenshots/                        # 7 genuine application screenshots
│       ├── 01_Dashboard.png
│       ├── 02_Shipment_Management.png
│       ├── 03_Shipment_Form.png
│       ├── 04_Route_Intelligence.png
│       ├── 05_Accessibility_Map.png
│       ├── 06_Accessibility_Reports.png
│       └── 07_Development_Environment.png
└── SmartLogisticsAccessibility.Web/        # ASP.NET Core MVC project
    ├── Controllers/                        # MVC Controllers
    ├── Data/                               # ApplicationDbContext & DbSeeder
    ├── Migrations/                         # Database migrations
    ├── Models/                             # C# Domain entity models
    ├── Services/                           # Business logic & recommendation engine
    ├── ViewModels/                         # Presentation models
    ├── Views/                              # Razor views & partials
    ├── wwwroot/                            # Static stylesheets, icons & scripts
    └── appsettings.json                    # Database connection string
```
