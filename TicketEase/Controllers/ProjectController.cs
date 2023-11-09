using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.DTO.Project;
using TicketEase.Application.Interfaces.Services;

namespace TicketEase.Controllers
{
    [Route("Project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;
        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpPost("AddProject")]
        public async Task<IActionResult> CreateProject(string boardId, [FromBody] ProjectRequestDto project)
        {
            return Ok(_projectServices.CreateProjectAsync(boardId, project)); 
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProject(string boardId, string projectId, [FromBody] UpdateProjectRequestDto projectUpdate)
        {
            return Ok(await _projectServices.UpdateProjectAsync(boardId, projectId, projectUpdate));
        }

    }
}
