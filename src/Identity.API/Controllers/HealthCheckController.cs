using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}