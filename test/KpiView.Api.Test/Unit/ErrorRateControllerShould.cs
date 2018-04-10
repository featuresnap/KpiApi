using System;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KpiView.Api.Test.Unit
{
    public class ErrorRateControllerShould
    {
        private KpiDbContext context;
        private ErrorRateController controller;

        public ErrorRateControllerShould()
        {
            var options = new DbContextOptionsBuilder<KpiDbContext>()
                .UseInMemoryDatabase("kpi").Options;
            context = new KpiDbContext(options);
            controller = new ErrorRateController(context);

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
                new CallOutcome {Id = 2, Timestamp = DateTime.Now.AddSeconds(-1.0), IsError = false});
            context.SaveChanges();

            var result = controller.Get();

            Assert.Equal(0.0M, result.Rate);
            
        }
    }
}