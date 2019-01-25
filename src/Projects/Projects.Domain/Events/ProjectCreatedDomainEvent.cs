using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Projects.Domain.AggregatesModel;

namespace Projects.Domain.Events
{
    public class ProjectCreatedDomainEvent:INotification
    {
        public Project Project { get; set; }
    }
}
