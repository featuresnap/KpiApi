using System;
using KpiView.Api.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KpiView.Api.Test.Unit
{
    public class ErrorRateControllerShould
    {
        private KpiDbContext _context;
        private ErrorRateController _controller;
        private Mock<ILoggingAdapter<ErrorRateController>> _logger;
        private long _nextId = 1;

        public ErrorRateControllerShould()
        {
            var dbOptions = new DbContextOptionsBuilder<KpiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new KpiDbContext(dbOptions);

            _logger = new Mock<ILoggingAdapter<ErrorRateController>>();
            _controller = new ErrorRateController(_context, _logger.Object);

        }

        [Theory]
        [InlineData(0, 0, 0.00)]
        [InlineData(1, 0, 0.00)]
        [InlineData(1, 99, 0.99)]
        [InlineData(99, 1, 0.01)]
        public void CalculateErrorRateCorrectly(int nonErrors, int errors, decimal expectedErrorRate)
        {
            for (var i = 0; i < nonErrors; i++)
            {
                ArrangeNonError();
            }
            for (var i = 0; i < errors; i++)
            {
                ArrangeError();
            }

            var result = _controller.Get();

            Assert.Equal(expectedErrorRate, result.Rate);
        }

        [Fact]
        public void NotConsiderRecordsOlderThanOneHour()
        {
            ArrangeError(record => record.EndTimestamp = DateTime.Now.AddMinutes(-61.0));

            var result = _controller.Get();

            Assert.Equal(0.0M, result.Rate);
            Assert.Equal(ColorCondition.GREEN, result.ColorCondition);
        }

        [Theory]
        [InlineData(0, 0, ColorCondition.GREEN)]
        [InlineData(99, 1, ColorCondition.GREEN)]
        [InlineData(98, 2, ColorCondition.YELLOW)]
        [InlineData(95, 5, ColorCondition.YELLOW)]
        [InlineData(94, 6, ColorCondition.RED)]
        public void AssignColorBasedOnErrorRate(int nonErrors, int errors, string expectedColor)
        {
            for (var i = 0; i < nonErrors; i++)
            {
                ArrangeNonError();
            }
            for (var i = 0; i < errors; i++)
            {
                ArrangeError();
            }

            var result = _controller.Get();

            Assert.Equal(expectedColor, result.ColorCondition);
        }

        [Fact]
        public void LogWarningWhenNoRecordsExist()
        {
            _controller.Get();
            _logger.Verify(l => l.LogWarning("No calls available for error rate computation"));
        }

        [Fact]
        public void LogErrorWhenRetrievingData()
        {
            var context = new Mock<KpiDbContext>();
            context.SetupGet(c => c.CallOutcomes).Throws(new TimeoutException());
            var controller = new ErrorRateController(context.Object, _logger.Object);
            Assert.ThrowsAny<Exception>(() => controller.Get());
            _logger.Verify(l => l.LogError(It.IsAny<TimeoutException>(), It.IsAny<string>()));
        }

        private void ArrangeError(Action<CallOutcome> overrides = null)
        {
            ArrangeRecord(true, overrides);
        }

        private void ArrangeNonError(Action<CallOutcome> overrides = null)
        {
            ArrangeRecord(false, overrides);
        }

        private void ArrangeRecord(bool isError, Action<CallOutcome> overrides = null)
        {
            var callOutcome = new CallOutcome { Id = _nextId++, EndTimestamp = DateTime.Now.AddSeconds(-1.0), IsError = isError };
            if (overrides != null) { overrides(callOutcome); }
            _context.CallOutcomes.Add(callOutcome);
            _context.SaveChanges();
        }
    }
}