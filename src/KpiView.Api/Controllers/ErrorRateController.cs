using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KpiView.Api
{
    [Route("api/kpis/[controller]")]
    public class ErrorRateController : Controller 
    {
        private KpiDbContext _dbContext;

        public ErrorRateController (KpiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public ErrorRate Get()
        {
            decimal rate = _dbContext.CallOutcomes.Count(co=>co.IsError) / _dbContext.CallOutcomes.Count();
            return new ErrorRate {Rate = rate};
        }
    }
}