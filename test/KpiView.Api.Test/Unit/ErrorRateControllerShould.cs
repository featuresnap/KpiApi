using Xunit;

namespace KpiView.Api.Test.Unit 
{
    public class ErrorRateControllerShould
    {
        [Fact]
        public void ProvideResponse() 
        {
            var controller = new ErrorRateController();
            var result = controller.Get();
            Assert.NotNull(result);
        }
    }
}
