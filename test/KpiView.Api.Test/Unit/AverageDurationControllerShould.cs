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
        public void ReturnCorrectAverageForTwoRecordsOfSameDuration()
        {
            ArrangeCallTaking(TimeSpan.FromSeconds(1.0));
            ArrangeCallTaking(TimeSpan.FromSeconds(1.0));
            var result = _controller.Get();
            Assert.Equal(1000M, result.AverageDurationMilliseconds);
        }

        [Fact]
        public void ReturnNullDurationWhenNoRecordsFound()
        {
            var result = _controller.Get();
            Assert.Null(result.AverageDurationMilliseconds);
        }

        [Fact]
        public void IncludeRecordsEndingWithin24Hours()
        {
            var startTime = DateTime.Now.AddHours(-24.1);
            var endTime = DateTime.Now.AddHours(-23.9);

            ArrangeCall(c =>
            {
                c.StartTime = startTime;
                c.EndTime = endTime;
                c.DurationMilliseconds = (decimal) (endTime - startTime).TotalMilliseconds;
            });

            var result = _controller.Get();

            Assert.True(result.AverageDurationMilliseconds > 0.0M);
        }

        [Fact]
        public void ExcludeRecordsEndingEarlierThan24HoursAgo()
        {
            var startTime = DateTime.Now.AddHours(-24.2);
            var endTime = DateTime.Now.AddHours(-24.1);

            ArrangeCall(c =>
            {
                c.StartTime = startTime;
                c.EndTime = endTime;
                c.DurationMilliseconds = (decimal) (endTime - startTime).TotalMilliseconds;
            });

            var result = _controller.Get();

            Assert.False(result.AverageDurationMilliseconds > 0.0M);
        }

        [Fact]
        public void LogWarningWhenNoRecordsFound()
        {
            _controller.Get();
            _logger.Verify(l => l.LogWarning(It.IsAny<string>()));
        }

        private void ArrangeCall(Action<CallDuration> configuration)
        {
            var callDuration = new CallDuration
            {
                Id = _nextId++
            };
            configuration(callDuration);
            _dbContext.CallDurations.Add(callDuration);
            _dbContext.SaveChanges();

        }
        private void ArrangeCallTaking(TimeSpan duration)
        {
            var endTime = DateTime.Now.AddMinutes(-1.0);
            var startTime = endTime - duration;
            ArrangeCall(c =>
            {
                c.DurationMilliseconds = (decimal) duration.TotalMilliseconds;
                c.EndTime = DateTime.Now.AddMinutes(-1.0);
                c.StartTime = startTime;
            });
        }
    }
}