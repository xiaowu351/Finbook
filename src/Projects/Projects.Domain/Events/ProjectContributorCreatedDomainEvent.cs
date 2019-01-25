using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Projects.Domain.AggregatesModel;

namespace Projects.Domain.Events
{
    public class ProjectContributorCreatedDomainEvent : INotification
    {

        public ProjectContributor Contributor { get; set; }
    }
}
