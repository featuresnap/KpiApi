using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KpiView.Api.Logging;

namespace KpiView.Api
{
    [Route("api/kpis/[controller]")]
    public class ErrorRateController : Controller
    {
        private KpiDbContext _dbContext;
        private ILoggingAdapter _logger;

        public ErrorRateController(KpiDbContext dbContext, ILoggingAdapter<ErrorRateController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public ErrorRate Get()
        {
            decimal allCalls = _dbContext.CallOutcomes.Count();
            decimal errorCalls = _dbContext.CallOutcomes.Count(co => co.IsError);
            var result = new ErrorRate();
            if (allCalls != 0)
            {
                result.Rate = errorCalls / allCalls;
            }
            else
            {
                _logger.LogWarning("No calls available for error rate computation");
            }
            return result;
        }
    }
}