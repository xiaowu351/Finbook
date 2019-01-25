using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Projects.API.Application.IntegrationEvents;
using Projects.API.Application.IntegrationEvents.Events;
using Projects.Domain.Events;

namespace Projects.API.Application.DomainEventHandlers
{
    /// <summary>
    /// 领域事件处理器
    /// 将接收到的领域事件转成集成事件发送至RabbitMQ
    /// </summary>
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedDomainEvent>
    {
        private readonly IProjectIntegrationEventService _projectIntegrationEventService;
        public ProjectCreatedDomainEventHandler(IProjectIntegrationEventService projectIntegrationEventService)
        {
            _projectIntegrationEventService = projectIntegrationEventService;
        }

        /// <summary>
        /// 将领域事件转成集成事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new ProjectCreatedIntegrationEvent() {
                ProjectId = notification.Project.Id,
                Company = notification.Project.Company,
                Avatar = notification.Project.Avatar,
                FromUserId = notification.Project.UserId,
                Introduction = notification.Project.Introduction
            };

            await _projectIntegrationEventService.PublishThroughEventBusAsync(integrationEvent);
        }
    }
}
