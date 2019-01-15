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
using System.Collections;
using User.API.ViewModels;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private AppUserContext _userContext;
        private ILogger<UsersController> _logger;


        public UsersController(AppUserContext userContext, ILogger<UsersController> logger)
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

            if (user is null)
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


        [HttpPost("check-or-create")]
        public async Task<IActionResult> CheckOrCreate([FromBody]CheckOrCreateInputViewModel viewModel)
        {
            var user = await _userContext.AppUsers.Where(u => u.Phone.Equals(viewModel.Phone)).SingleOrDefaultAsync();
            if (user == null)
            {
                _userContext.AppUsers.Add(new AppUser { Phone = viewModel.Phone, Name = viewModel.Phone });
                await _userContext.SaveChangesAsync();
            }
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Company,
                user.Title,
                user.Avatar
            });
        }


        /// <summary>
        /// 获取用户标签列表
        /// </summary>      
        /// <returns></returns> 
        [HttpGet("tags")]
        public async Task<IActionResult> GetUserTags()
        {
            return Ok(await _userContext.UserTags.Where(u => u.AppUserId == UserIdentity.UserId).ToListAsync());
        }

        /// <summary>
        /// 通过手机号搜索
        /// </summary>
        /// <param name="phone"></param>
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]SearchInputViewModel viewModel)
        {
            return Ok(await _userContext.AppUsers.Include(u => u.Properties).SingleOrDefaultAsync(u => u.Phone == viewModel.Phone));
        }

        /// <summary>
        /// 更新用户标签列表
        /// </summary>       
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPut("tags")]
        public async Task<IActionResult> UpdateUserTags([FromBody]List<string> tags)
        {
            var originTags = await _userContext.UserTags.Where(u => u.AppUserId == UserIdentity.UserId).Select(t => t.Tag).ToListAsync();

            var newTags = tags.Except(originTags);

            _userContext.UserTags.AddRange(newTags.Select(t => new UserTag
            {
                AppUserId = UserIdentity.UserId,
                Tag = t,
                CreateTime = DateTime.Now
            }));

            await _userContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("baseInfo/{userId}")]
        public async Task<IActionResult> GetBaseUserInfo(int userId)
        {
            // TBD 检查用户是否好友关系

            var user = await _userContext.AppUsers.Where(u => u.Id == userId).SingleOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                userId = user.Id,
                user.Name,
                user.Company,
                user.Title,
                user.Avatar
            });
        }

    }
}
