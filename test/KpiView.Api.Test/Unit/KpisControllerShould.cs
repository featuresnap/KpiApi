using System;
using KpiView.Api.Controllers;
using Xunit;

namespace KpiView.Api.Test.Unit {
    public class KpisControllerShould {
        [Fact]
        public void RespondToGetRequest () {
            var controller = new KpisController ();
            var result = controller.Get ();
            Assert.NotNull (result);
        }

        [Fact]
        public void ListErrorRateEndpoint () {
            var controller = new KpisController ();

            var result = controller.Get ();

            Assert.Contains (result, endpoint =>
                endpoint.Name == "Error Rate" &&
                endpoint.Description == "Average error rate for the preceding hour" &&
                endpoint.Route == "api/kpis/errorRate"
            );
        }

        [Fact]
        public void ListAverageDurationEndpoint () {
            var controller = new KpisController ();
            
            var result = controller.Get ();

            Assert.Contains (result, endpoint =>
                endpoint.Name == "Average Request Duration" &&
                endpoint.Description == "Average duration of requests for the preceding 24 hours" &&
                endpoint.Route == "api/kpis/averageDuration"
            );
        }

    }
}