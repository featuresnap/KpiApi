using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace KpiView.Api.Controllers 
{
    [Route("api/[controller]")]
    public class KpisController : Controller
    {
        public IEnumerable<KpiDescription> Get() 
        {
            return new KpiDescription[] 
            {
                new KpiDescription 
                {
                    Name = "Error Rate", 
                    Description = "Average error rate for the preceding hour",
                    Route = "api/kpis/errorRate"},
                new KpiDescription
                {
                    Name = "Average Request Duration",
                    Description = "Average duration of requests for the preceding 24 hours",
                    Route = "api/kpis/averageDuration"
                }
                
            };
        }
    }
}