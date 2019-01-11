using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Dtos;
using Contact.API.Exceptions;
using Contact.API.Models;
using Contact.API.Services;
using Contact.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private UserIdentity UserIdentity => new UserIdentity { UserId = 1 };

        private IContactApplyRequestRepository _contactApplyRequestRepository;
        private IContactRepository _contactRepository;
        private IUserService _userService;
        public ContactsController(IContactApplyRequestRepository contactApplyRequestRepository, 
            IUserService userService,
            IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _contactRepository = contactRepository;
            _userService = userService;
        }
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _contactRepository.GetContactsAsync(UserIdentity.UserId));
        }

        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("apply-requests")]
        public async Task<IActionResult> GetApplyRequests()
        {
            return Ok(await _contactApplyRequestRepository.GetReuestListAsync(UserIdentity.UserId));
        }

        /// <summary>
        /// 联系人标签更新
        /// </summary>
        /// <param name="contactTags"></param>
        /// <returns></returns>
        [HttpPut("tags")]
        public async Task<IActionResult> ContactTags([FromBody]ContactTagsInputViewModel contactTags)
        {
            var result = await _contactRepository.UpdateContactTagsAsync(UserIdentity.UserId, contactTags.contactId, contactTags.Tags);
            if (!result)
            {
                // log TBD
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPost("apply-requests")]
        public async Task<IActionResult> AddApplyRequest([FromBody]AddApplyRequestInputViewModel viewModel)
        {
            var userId = viewModel.UserId;
            if(UserIdentity.UserId == userId)
            {
                throw new ContactOperationException("不能向自己发送的好友请求");
            }
            var baseUserInfo = await _userService.GetUserInfoAsync(userId);

            if (baseUserInfo == null)
            {
                throw new ContactOperationException("用户参数错误");
            }

            var result = await _contactApplyRequestRepository.AddRequestAsync(new ContactApplyRequest
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = baseUserInfo.Name,
                Company = baseUserInfo.Company,
                Title = baseUserInfo.Title,
                ApplyTime = DateTime.Now,
                Avatar = baseUserInfo.Avatar
            });

            if (!result)
            {
                //log TBD
                return BadRequest();
            } 
            return Ok();
        }

       /// <summary>
       /// 通过好友的请求
       /// </summary>
       /// <param name="applierId">请求的好友ID</param>
       /// <returns></returns>
        [HttpPut("apply-requests")]
        public async Task<IActionResult> ApprovalApplyRequest([FromBody]ApprovalApplyRequestInputViewModel  viewModel)
        {
            var applierId = viewModel.ApplierId;
            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId);
            if (!result)
            {
                // log TBD
                return BadRequest();
            }

            //这里需要注意，添加好友请求的时候，需要完成双向的好友关系
            //1.完成自身的好友关系
            var applier = await _userService.GetUserInfoAsync(applierId);
            var userResult = await _contactRepository.AddContactInfoAsync(UserIdentity.UserId, applier);

            if (!userResult)
            {
                // log TBD
                return BadRequest();
            }

            //2.完成申请人的好友关系
            var userInfo = await _userService.GetUserInfoAsync(UserIdentity.UserId);
            var applierResult = await _contactRepository.AddContactInfoAsync(applierId, userInfo);

            if (!applierResult)
            {
                //log TBD
                return BadRequest();
            }

            return Ok();
        }
    }
}
