using System;
using System.Linq;
using KpiView.Api;
using KpiView.Api.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KpiView.Api.Test
{
    public class CallDurationControllerShould
    {
        private KpiDbContext _dbContext;
        private Mock<ILoggingAdapter<CallDurationController>> _logger;
        private CallDurationController _controller;

        public CallDurationControllerShould()
        {
            var dbOptions = new DbContextOptionsBuilder<KpiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _dbContext = new KpiDbContext(dbOptions);
            _logger = new Mock<ILoggingAdapter<CallDurationController>>();

            _controller = new CallDurationController(_dbContext, _logger.Object);
        }

        [Fact]
        public void Plumbing() { }
    }
}