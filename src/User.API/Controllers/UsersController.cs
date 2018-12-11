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
using Microsoft.AspNetCore.JsonPatch;
using User.API.Models;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private AppUserContext _userContext;
        private ILogger<UsersController> _logger;

        private UserIdentity UserIdentity => new UserIdentity { UserId =1, Name = "test" };

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
        /// <summary>
        /// 有两种方式可以实现UserProperties属性的更新：
        /// 1. 先删除原来，然后再插入最新的；（简单）
        /// 2. 先查询原来的，然后对比最新的，再执行删除和插入；（复杂）以下是使用此方法的实现
        /// </summary>
        /// <param name="patchUser"></param>
        /// <returns></returns>
        // POST api/values
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<AppUser> patchUser)
        {
            var user = await _userContext.AppUsers 
                                         .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            //查找原始记录
            var originProperties = await _userContext.UserProperties.Where(up => up.AppUserId == user.Id).ToListAsync();

            patchUser.ApplyTo(user);
            //统计所有记录（新+旧）
            var allProperties = originProperties.Union(user.Properties).Distinct();
            //统计移除的记录
            var removeProperties = originProperties.Except(user.Properties);
            //统计添加的记录
            var addProperties = allProperties.Except(originProperties);

            foreach (var property in removeProperties)
            {
                _userContext.UserProperties.Remove(property);
            }
            foreach (var property in addProperties)
            {
                _userContext.UserProperties.Add(property);
            }
            
            await _userContext.SaveChangesAsync();

            
            //返回内容待后续需求确定再修改
            return Json(user);

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
