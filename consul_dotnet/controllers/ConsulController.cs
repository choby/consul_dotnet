using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace consul_dotnet.controllers
{
    [Produces("application/json")]
    [Route("api/health")]
    public class ConsulController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}

