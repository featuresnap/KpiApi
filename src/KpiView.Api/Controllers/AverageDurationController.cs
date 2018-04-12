using System;
using System.Linq;
using KpiView.Api.Logging;
using Microsoft.AspNetCore.Mvc;

namespace KpiView.Api
{
    [Route("api/kpis/[controller]")]
    public class AverageDurationController : Controller
    {
        private readonly KpiDbContext _dbContext;
        private readonly ILoggingAdapter _logger;

        public AverageDurationController(KpiDbContext dbContext, ILoggingAdapter<AverageDurationController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public AverageDurationResponse Get()
        {
            var oldestEndTime = DateTime.Now.AddHours(-24.0);
            var records = _dbContext.CallDurations.Count(c=>c.EndTime >= oldestEndTime);
            var response = new AverageDurationResponse();
            decimal? averageDuration = null;
            if (records > 0) {
                averageDuration = _dbContext.CallDurations
                .Where(c=>c.EndTime >= oldestEndTime)
                .Select(x=>x.DurationMilliseconds)
                .Average();
            }
        
            return new AverageDurationResponse{AverageDurationMilliseconds = averageDuration};
        }
    }

}