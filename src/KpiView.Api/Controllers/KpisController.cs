using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace KpiView.Api.Controllers 
{
    [Route("api/[controller]")]
    public class KpisController : Controller
    {
        public IEnumerable<string> Get() 
        {
            return new string[] {};
        }
    }
}