using Microsoft.EntityFrameworkCore;

namespace KpiView.Api
{
    public class KpiDbContext : DbContext
    {
        protected KpiDbContext() : base()
        {

        }
        public KpiDbContext(DbContextOptions<KpiDbContext> options) : base(options)
        {

        }

        public virtual DbSet<CallOutcome> CallOutcomes { get; set; }
    }
}