using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Dtos;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private AppUserContext _userContext;
        private ILogger<UsersController> _logger;

        private UserIdentity UserIdentity => new UserIdentity { UserId = 2, Name = "test" };

        public UsersController(AppUserContext userContext,ILogger<UsersController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }


        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userContext.AppUsers
                                    .AsNoTracking()
                                    .Include(u => u.Properties)
                                    .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            if(user is null)
            {
                //return NotFound();
                throw new UserOperationException($"找不到用户上下文：{UserIdentity.UserId}");
            }
            
            return Json(user);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
