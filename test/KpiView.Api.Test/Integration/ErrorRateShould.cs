using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KpiView.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace KpiView.Api.Test.Integration
{
    public class ErrorRateShould
    {
        [Fact]
        public async Task RespondWithOkHttpStatus()
        {
            var hostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            using(var server = new TestServer(hostBuilder))
            {
                HttpClient client = server.CreateClient();

                var response = await client.GetAsync("/api/kpis/errorRate");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}