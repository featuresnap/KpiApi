using System;
using KpiView.Api.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KpiView.Api.Test.Unit
{
    public class ErrorRateControllerShould
    {
        private KpiDbContext context;
        private ErrorRateController controller;
        private Mock<ILoggingAdapter<ErrorRateController>> _logger;

        public ErrorRateControllerShould()
        {
            var options = new DbContextOptionsBuilder<KpiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _logger = new Mock<ILoggingAdapter<ErrorRateController>>();
            context = new KpiDbContext(options);
            controller = new ErrorRateController(context, _logger.Object);

        }

        [Fact]
        public void Return100PercentWhenOnlyOneFailedRecordExists()
        {
            context.CallOutcomes.Add(
                new CallOutcome { Id = 1, Timestamp = DateTime.Now.AddSeconds(-1.0), IsError = true });
            context.SaveChanges();

            var result = controller.Get();

            Assert.Equal(1.0M, result.Rate);
        }

        [Fact]
        public void ReturnZeroWhenOnlyOneSuccessRecordExists()
        {
            context.CallOutcomes.Add(
                new CallOutcome { Id = 2, Timestamp = DateTime.Now.AddSeconds(-1.0), IsError = false });
            context.SaveChanges();

            var result = controller.Get();

            Assert.Equal(0.0M, result.Rate);
        }

        [Fact]
        public void ReturnZeroWhenNoRecordsExist()
        {
            var result = controller.Get();

            Assert.Equal(0.0M, result.Rate);
        }

        [Fact]
        public void LogWarningWhenNoRecordsExist()
        {
            controller.Get();
            _logger.Verify(l => l.LogWarning("No calls available for error rate computation"));
        }
    }
}