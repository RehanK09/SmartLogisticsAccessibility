# AI-Based Smart Logistics and Accessibility Intelligence Platform

**Course:** CSE253 (.NET) — CA3 Allocated Project  
**Project Code:** SIH26002  

## 👥 Team Members
- **Peta Venkata Ashok** — `RK3P25RAA28`
- **Rehan Khan** — `RK3P25RAA29`
- **Ishaan Sehdev** — `RK3P25RAA30`

---

## 🛠️ Technology Stack
- **Framework:** ASP.NET Core MVC (.NET 10 / `net10.0`)
- **ORM:** Entity Framework Core `10.0.15`
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `Microsoft.EntityFrameworkCore.Design`
- **Database:** Microsoft SQL Server 2025 Express (`QUANTUM\SQLEXPRESS`)
- **IDE:** Visual Studio Community 2026 / Visual Studio Code

---

## 📂 Project Structure
```text
SmartLogisticsAccessibility/
├── SmartLogisticsAccessibility.sln         # Visual Studio Solution
├── SmartLogisticsAccessibility.slnx        # Modern XML Solution
└── SmartLogisticsAccessibility.Web/        # ASP.NET Core MVC Project
    ├── Controllers/                        # MVC Controllers
    ├── Data/                               # Entity Framework DbContext
    │   └── ApplicationDbContext.cs
    ├── Models/                             # ViewModels and Domain Entities
    ├── Views/                              # Razor MVC Views
    ├── wwwroot/                            # Static assets (Bootstrap, CSS, JS)
    ├── appsettings.json                    # Configuration & Connection Strings
    └── Program.cs                          # Application Startup Pipeline
```

---

## 🚀 Getting Started

### Prerequisites
1. [.NET 10 SDK / Runtime](https://dotnet.microsoft.com/)
2. [Visual Studio Community 2026](https://visualstudio.microsoft.com/) (with *ASP.NET and web development* workload)
3. [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) & [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### Running the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/RehanK09/SmartLogisticsAccessibility.git
   cd SmartLogisticsAccessibility
   ```
2. Restore NuGet dependencies and build:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Run the web application:
   ```bash
   dotnet run --project SmartLogisticsAccessibility.Web
   ```
4. Or open `SmartLogisticsAccessibility.sln` in **Visual Studio Community** and press <kbd>F5</kbd>.
