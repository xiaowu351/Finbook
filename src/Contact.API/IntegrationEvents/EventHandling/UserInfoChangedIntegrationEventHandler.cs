using Contact.API.Data;
using Contact.API.IntegrationEvents.Events;
using Finbook.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.IntegrationEvents.EventHandling
{
    public class UserInfoChangedIntegrationEventHandler : IIntegrationEventHandler<UserInfoChangedIntegrationEvent>
    {
        private readonly IContactRepository _contactRepository;
        public UserInfoChangedIntegrationEventHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;

        }

        public async Task Handle(UserInfoChangedIntegrationEvent @event)
        {
            await _contactRepository.UpdateContactInfoAsync(new Dtos.UserIdentity
            {
                UserId = @event.UserId,
                Name = @event.Name,
                Title = @event.Title,
                Company = @event.Company,
                Avatar = @event.Avatar
            });
        }
    }
}
