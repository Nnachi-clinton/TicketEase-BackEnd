using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Application.DTO.Project;
using TicketEase.Domain;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IProjectServices
    {
        Task<ApiResponse<ProjectReponseDto>> CreateProjectAsync(string boardId, ProjectRequestDto project);
        Task<ApiResponse<ProjectReponseDto>> UpdateProjectAsync(string boardId, string projectId, UpdateProjectRequestDto projectUpdate);
    }
}
