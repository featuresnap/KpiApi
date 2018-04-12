using System;
using System.Linq;
using KpiView.Api.Logging;
using Microsoft.AspNetCore.Mvc;

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

        public ErrorRateResponse Get()
        {
            var (allCalls, errorCalls) = CountTotalAndErrorRecords();
            var response = new ErrorRateResponse();
            response.Rate = CalculateErrorRate(allCalls, errorCalls);
            response.ColorCondition = GetColorCondition(response.Rate);
            return response;
        }

        private(int, int) CountTotalAndErrorRecords()
        {
            try
            {
                var oldestTimestamp = DateTime.Now.AddHours(-1.0);
                var allCalls = _dbContext.CallOutcomes.Count(c => c.Timestamp >= oldestTimestamp);
                var errorCalls = _dbContext.CallOutcomes.Count(c => c.Timestamp >= oldestTimestamp && c.IsError);
                return (allCalls, errorCalls);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to retrieve record counts for error rate computation");
                throw;
            }
        }

        private decimal CalculateErrorRate(int allCalls, int errorCalls)
        {
            if (allCalls == 0)
            {
                _logger.LogWarning("No calls available for error rate computation");
                return 0.0M;
            }
            return (decimal) errorCalls / (decimal) allCalls;
        }

        private string GetColorCondition(decimal errorRate)
        {
            if (errorRate <= 0.01M) { return ColorCondition.GREEN; }
            if (errorRate <= 0.05M) { return ColorCondition.YELLOW; }
            return ColorCondition.RED;
        }

    }
}