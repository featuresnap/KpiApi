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
            return new AverageDurationResponse { AverageDurationMilliseconds = ComputeAverageDuration() };
        }

        private decimal? ComputeAverageDuration()
        {
            var oldestEndTime = DateTime.Now.AddHours(-24.0);
            var eligibleRecords = _dbContext.CallDurations.Where(c => c.EndTime >= oldestEndTime);

            if (!eligibleRecords.Any())
            {
                _logger.LogWarning("No records available for average duration computation.");
                return null;
            }

            return eligibleRecords
                .Select(x => x.DurationMilliseconds)
                .Average();
        }
    }

}