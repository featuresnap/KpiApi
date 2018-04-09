using KpiView.Api;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Net;

namespace KpiView.Api.Test.Integration
{
    public class KpiApiShould
    {
        [Fact]
        public async Task ListKpis()
        {
            var hostBuilder = new WebHostBuilder()
            .UseStartup<Startup>();

            using (var server = new TestServer(hostBuilder)) 
            {
                HttpClient client = server.CreateClient();
                
                var response = await client.GetAsync("/api/kpis");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
        
    }


}
