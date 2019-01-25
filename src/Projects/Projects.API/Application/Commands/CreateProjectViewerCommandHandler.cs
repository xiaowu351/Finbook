using MediatR;
using Projects.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projects.API.Application.Commands
{
    public class CreateProjectViewerCommandHandler : IRequestHandler<CreateProjectViewerCommand>
    {
        private readonly IProjectRepository _projectRepository;
        public CreateProjectViewerCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(CreateProjectViewerCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);

            if (project.UserId == request.UserId)
            {
                throw new Domain.Exceptions.ProjectDomainException(" you cannot join your own project.");
            }

            project.AddViewer(request.UserId, request.UserName, request.Avatar);

            //_projectRepository.UpdateProject(project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
