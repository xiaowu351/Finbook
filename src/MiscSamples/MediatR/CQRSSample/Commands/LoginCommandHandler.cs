using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CQRSSample.DomainEventHandlers;


namespace CQRSSample.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly IMediator _mediator;
        public LoginCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _mediator.Publish(new LoginSuccessedDomainEvent { UserName = request.UserName });
            return Task.FromResult(true);
        }
    }
}
