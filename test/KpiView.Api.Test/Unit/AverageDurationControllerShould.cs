using System;
using System.Linq;
using KpiView.Api;
using KpiView.Api.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KpiView.Api.Test
{
    public class AverageDurationControllerShould
    {
        private KpiDbContext _dbContext;
        private Mock<ILoggingAdapter<AverageDurationController>> _logger;
        private AverageDurationController _controller;
        private long _nextId;

        public AverageDurationControllerShould()
        {
            var dbOptions = new DbContextOptionsBuilder<KpiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _dbContext = new KpiDbContext(dbOptions);
            _logger = new Mock<ILoggingAdapter<AverageDurationController>>();

            _controller = new AverageDurationController(_dbContext, _logger.Object);
        }

        [Fact]
        public void ReturnCorrectAverageForSingleRecord()
        {
            ArrangeCallTaking(TimeSpan.FromSeconds(1.0));
            var result = _controller.Get();
            Assert.Equal(1000M, result.AverageDurationMilliseconds);
        }

        [Fact]
        public void ReturnNullDurationWhenNoRecordsFound()
        {
            var result =_controller.Get();
            Assert.Null(result.AverageDurationMilliseconds);
        }

        private void ArrangeCallTaking(TimeSpan duration, Action<CallDuration> extraConfiguration = null)
        {
            var endTime = DateTime.Now.AddMinutes(-1.0);
            var startTime = endTime - duration;
            var callDuration = new CallDuration
            {
                Id = _nextId++,
                DurationMilliseconds = (decimal) duration.TotalMilliseconds,
                EndTime = DateTime.Now.AddMinutes(-1.0),
                StartTime = startTime
            };
            if (extraConfiguration != null) { extraConfiguration(callDuration); }
            _dbContext.CallDurations.Add(callDuration);
            _dbContext.SaveChanges();
        }
    }
}