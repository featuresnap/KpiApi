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

        public AverageDurationController(KpiDbContext dbContext, ILoggingAdapter logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public AverageDurationResponse Get()
        {
            var records = _dbContext.CallDurations.Count();
            var averageDuration = _dbContext.CallDurations.Select(x=>x.DurationMilliseconds).DefaultIfEmpty(0).Average();
            return new AverageDurationResponse{AverageDurationMilliseconds = averageDuration};
        }
    }

}