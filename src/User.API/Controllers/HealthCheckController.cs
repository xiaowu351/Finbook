using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}