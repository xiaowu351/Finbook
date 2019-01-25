using MediatR;
using Projects.Domain.AggregatesModel;
using Projects.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projects.API.Application.Commands
{
    public class CreateProjectJoinCommandHandler : IRequestHandler<CreateProjectJoinCommand>
    {
        private readonly IProjectRepository _projectRepository;
        public CreateProjectJoinCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Unit> Handle(CreateProjectJoinCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);

            if(project == null)
            {
                throw new ProjectDomainException($"project not found：{request.Contributor.ProjectId}");
            }

            if(project.UserId == request.Contributor.UserId)
            {
                throw new ProjectDomainException(" you cannot join your own project.");
            }

            project.AddContributor(request.Contributor);

            //_projectRepository.UpdateProject(project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
