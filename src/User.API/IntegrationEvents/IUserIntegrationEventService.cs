using Finbook.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.IntegrationEvents
{
    public interface IUserIntegrationEventService
    {
        Task SaveEventAndContactContextChangeAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
