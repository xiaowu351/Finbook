using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Projects.Domain.AggregatesModel;

namespace Projects.API.Application.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IProjectRepository _projectRepository;
        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.AddProjectAsync(request.Project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return project;
        }
    }
}
