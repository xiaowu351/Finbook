using Finbook.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.IntegrationEvents
{
    public interface IContactIntegrationEventService
    {
        Task SaveEventAndContactContextChangeAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
