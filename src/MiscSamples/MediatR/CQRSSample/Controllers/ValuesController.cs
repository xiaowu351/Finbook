using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSSample.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        } 

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginCommand login)
        {
            var result = await _mediator.Send(login);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        } 
        
    }
}
