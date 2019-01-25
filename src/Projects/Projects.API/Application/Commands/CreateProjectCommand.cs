using MediatR;
using Projects.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects.API.Application.Commands
{
    public class CreateProjectCommand : IRequest<Project>
    {
        public Project Project { get; set; }
    }
}
