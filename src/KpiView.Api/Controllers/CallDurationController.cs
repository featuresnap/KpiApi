using System;
using KpiView.Api.Logging;
using Microsoft.AspNetCore.Mvc;

namespace KpiView.Api
{
    public class CallDurationController : Controller
    {
        private readonly KpiDbContext _dbContext;
        private readonly ILoggingAdapter _logger;

        public CallDurationController(KpiDbContext dbContext, ILoggingAdapter logger)
        {
            _dbContext = dbContext; 
            _logger = logger;
        }
    }

}