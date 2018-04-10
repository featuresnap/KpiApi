using System;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KpiView.Api.Test.Unit {
    public class ErrorRateControllerShould {

        [Fact]
        public void Return100PercentWhenOnlyOneFailedRecordExists () 
        {
            var options = new DbContextOptionsBuilder<KpiDbContext> ()
                .UseInMemoryDatabase ("kpi").Options;
            var context = new KpiDbContext (options);
            context.CallOutcomes.Add (
                new CallOutcome { Id = 1, Timestamp = DateTime.Now.AddSeconds (-1.0), IsError = true });
            context.SaveChanges();
            var controller = new ErrorRateController (context);

            var result = controller.Get ();

            Assert.Equal(1.0M, result.Rate);
        }
    }
}