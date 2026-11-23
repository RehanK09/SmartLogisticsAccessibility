using Microsoft.EntityFrameworkCore;

namespace SmartLogisticsAccessibility.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
