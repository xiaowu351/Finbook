using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Projects.API.Application.Commands;
using Projects.API.Application.Queries;
using Projects.API.Application.Services;
using Projects.Domain.AggregatesModel;
using Projects.Domain.Exceptions;

namespace Projects.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IRecommendService _recommendService;
        private readonly IProjectQueries _projectQueries;
        public ProjectsController(IMediator mediator, IRecommendService recommendService, IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _projectQueries.GetProjectsByUserIdAsync(UserIdentity.UserId);
            return Ok(result);
        }

        [HttpGet("my/{projectId}")]
        public async Task<IActionResult> GetMyProjectDetails(int projectId)
        {
            var project = await _projectQueries.GetProjectDetailsAsync(projectId);

            if (project == null)
            {
                return BadRequest("该项目不存在.");
            }

            if (project.UserId == UserIdentity.UserId)
            {
                return Ok(project);
            }
            else
            {
                return BadRequest("无权查看该项目.");
            }
        }

        [HttpGet("recommends/{projectId}")]
        public async Task<IActionResult> GetRecommendProjectDetails(int projectId)
        {
            if (!await _recommendService.IsRecommendProject(projectId, UserIdentity.UserId))
            {
                return BadRequest("无权查看该项目");
            }
            else
            {
                var project = await _projectQueries.GetProjectDetailsAsync(projectId);
                return Ok(project);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            project.UserId = UserIdentity.UserId;
            var command = new CreateProjectCommand { Project = project };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            // 检查是否有权限查看
            if (!await _recommendService.IsRecommendProject(projectId, UserIdentity.UserId))
            {
                return BadRequest("无权查看此项目");
            }

            var command = new CreateProjectViewerCommand
            {
                UserId = UserIdentity.UserId,
                UserName = UserIdentity.Name,
                Avatar = UserIdentity.Avatar,
                ProjectId = projectId
            };

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("join/{projectId}")]
        public async Task<IActionResult> JoinProject(int projectId, [FromBody] ProjectContributor contributor)
        {
            
            contributor.ProjectId = projectId;
            contributor.UserId = UserIdentity.UserId;
            contributor.UserName = UserIdentity.Name;
            contributor.Avatar = UserIdentity.Avatar;
            // 检查是否有权限查看
            if (!await _recommendService.IsRecommendProject(contributor.ProjectId, UserIdentity.UserId))
            {
                return BadRequest("无权查看此项目");
            } 

            var cmd = new CreateProjectJoinCommand { Contributor = contributor };

            await _mediator.Send(cmd);
            return Ok();
        }
    }
}
