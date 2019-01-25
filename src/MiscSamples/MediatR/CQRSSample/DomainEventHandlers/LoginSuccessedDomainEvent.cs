using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace CQRSSample.DomainEventHandlers
{
    public class LoginSuccessedDomainEvent:INotification
    {
        public string UserName { get; set; }
    }
}
