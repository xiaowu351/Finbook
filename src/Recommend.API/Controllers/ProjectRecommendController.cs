using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Recommend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectRecommendController : BaseController
    {
        private readonly Data.ProjectRecommendContext _projectRecommendContext;
        public ProjectRecommendController(Data.ProjectRecommendContext projectRecommendContext)
        {
            _projectRecommendContext = projectRecommendContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _projectRecommendContext.ProjectRecommends
                                     .Where(pr => pr.UserId == UserIdentity.UserId)
                                     .ToListAsync();

            return Ok(result);
        } 
    }
}
