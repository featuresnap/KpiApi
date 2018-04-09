using System;
using KpiView.Api.Controllers;
using Xunit;

namespace KpiView.Api.Test
{
    public class KpisControllerShould
    {
        [Fact]
        public void DetermineListOfKpis()
        {
            var controller = new KpisController();
            var result = controller.Get();
            Assert.NotNull(result);
        }
    }
}
