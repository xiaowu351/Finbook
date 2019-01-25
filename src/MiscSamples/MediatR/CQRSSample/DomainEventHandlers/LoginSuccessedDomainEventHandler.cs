using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CQRSSample.DomainEventHandlers
{
    public class LoginSuccessedDomainEventHandler : INotificationHandler<LoginSuccessedDomainEvent>
    {
        public Task Handle(LoginSuccessedDomainEvent notification, CancellationToken cancellationToken)
        {
            // TBD

            //1. 记录登录成功的日志
            //2. 将领域事件发送至EventBus

            return Task.CompletedTask;
        }
    }
}
