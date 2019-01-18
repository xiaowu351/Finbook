using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbook.BuildingBlocks.EventBus.Abstractions;
using Finbook.BuildingBlocks.EventBus.Events;

namespace User.API.IntegrationEvents
{
    /// <summary>
    /// 往EventBus发送数据
    /// </summary>
    public class UserIntegrationEventService : IUserIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        public UserIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            _eventBus.Publish(evt);
            return Task.CompletedTask;
        }

        public Task SaveEventAndContactContextChangeAsync(IntegrationEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}
