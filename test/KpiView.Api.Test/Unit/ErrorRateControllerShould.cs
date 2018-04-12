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
            var options = new DbContextOptionsBuilder<KpiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _logger = new Mock<ILoggingAdapter<ErrorRateController>>();
            _context = new KpiDbContext(options);
            _controller = new ErrorRateController(_context, _logger.Object);

        }

        [Fact]
        public void Return100PercentWhenOnlyOneFailedRecordExists()
        {
            ArrangeError();

            var result = _controller.Get();

            Assert.Equal(1.0M, result.Rate);
        }

        [Fact]
        public void ReturnZeroWhenOnlyOneSuccessRecordExists()
        {
            ArrangeNonError();

            var result = _controller.Get();

            Assert.Equal(0.0M, result.Rate);
        }

        [Fact]
        public void ReturnZeroWhenNoRecordsExist()
        {
            var result = _controller.Get();

            Assert.Equal(0.0M, result.Rate);
        }

        [Fact]
        public void ReturnGreenWhenErrorRateBelow1Percent()
        {
            ArrangeError();
            for (var i = 0; i < 99; i++)
            {
                ArrangeNonError();
            }
            
            var result = _controller.Get();

            Assert.Equal(0.01M, result.Rate);
        }

        [Fact]
        public void LogWarningWhenNoRecordsExist()
        {
            _controller.Get();
            _logger.Verify(l => l.LogWarning("No calls available for error rate computation"));
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
            var callOutcome = new CallOutcome { Id = _nextId++, Timestamp = DateTime.Now.AddSeconds(-1.0), IsError = isError };
            if (overrides != null) { overrides(callOutcome); }
            _context.CallOutcomes.Add(callOutcome);
            _context.SaveChanges();
        }

    }
}