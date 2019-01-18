using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbook.BuildingBlocks.EventBus.Events;

namespace Contact.API.IntegrationEvents
{
    public class ContactIntegrationEventService : IContactIntegrationEventService
    {
        public Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            throw new NotImplementedException();
        }

        public Task SaveEventAndContactContextChangeAsync(IntegrationEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}
