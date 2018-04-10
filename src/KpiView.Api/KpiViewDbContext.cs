using Microsoft.EntityFrameworkCore;

namespace KpiView.Api
{
    public class KpiDbContext : DbContext 
    {
        public KpiDbContext(DbContextOptions<KpiDbContext> options): base(options)
        {
            
        }

        public DbSet<CallOutcome> CallOutcomes {get;set;}
    }
}