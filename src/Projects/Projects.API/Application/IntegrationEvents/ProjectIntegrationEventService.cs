using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbook.BuildingBlocks.EventBus.Abstractions;
using Finbook.BuildingBlocks.EventBus.Events;

namespace Projects.API.Application.IntegrationEvents
{
    public class ProjectIntegrationEventService : IProjectIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        public ProjectIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        /// <summary>
        /// 发送集成事件至RabbitMQ
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            _eventBus.Publish(evt);
            return Task.CompletedTask;
        }
    }
}
