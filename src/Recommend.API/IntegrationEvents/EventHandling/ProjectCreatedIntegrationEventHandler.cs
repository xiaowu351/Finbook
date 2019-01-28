using Finbook.BuildingBlocks.EventBus.Abstractions;
using Recommend.API.Data;
using Recommend.API.IntegrationEvents.Events;
using Recommend.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommend.API.Services;

namespace Recommend.API.IntegrationEvents.EventHandling
{
    public class ProjectCreatedIntegrationEventHandler : IIntegrationEventHandler<ProjectCreatedIntegrationEvent>
    {
        private readonly ProjectRecommendContext _recommendContext;
        private readonly IUserService _userService;
        private readonly IContactService _contactService;

        public ProjectCreatedIntegrationEventHandler(ProjectRecommendContext recommendContext,
            IUserService userService,
            IContactService contactService)
        {
            _recommendContext = recommendContext;
            _userService = userService;
            _contactService = contactService;
        }
        public async Task Handle(ProjectCreatedIntegrationEvent @event)
        { 
            //1.获取用户信息
            var user = await _userService.GetBaseUserInfoAsync(@event.FromUserId); 
           
            //2.获取好友列表
            var contacts = await _contactService.GetContactsAsync(@event.FromUserId);
            foreach (var item in contacts)
            {
                var projectRecommend = new ProjectRecommend
                {
                    ProjectId = @event.ProjectId,
                    ProjectAvatar = @event.Avatar,
                    Company = @event.Company,
                    Introduction = @event.Introduction,
                    FinStage = @event.FinStage,
                    RecommendTime = DateTime.Now,
                    RecommendType = EnumRecommendType.Friend,
                    FromUserId = @event.FromUserId,
                    CreatedTime = DateTime.Now,
                    Tags = @event.Tags,
                     FromUserName = user.Name,
                     UserId = item.UserId 
                };
                _recommendContext.Add(projectRecommend);
            } 
            
            await _recommendContext.SaveChangesAsync();
        }
    }
}
