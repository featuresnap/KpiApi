using Microsoft.AspNetCore.Mvc;

namespace KpiView.Api
{
    [Route("api/kpis/[controller]")]
    public class ErrorRateController : Controller 
    {
        public string Get()
        {
            return "Foo";
        }
    }
}