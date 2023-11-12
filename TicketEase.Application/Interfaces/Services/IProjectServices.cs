using TicketEase.Application.DTO.Project;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IProjectServices
    {
        Task<ApiResponse<ProjectReponseDto>> CreateProjectAsync(string boardId, ProjectRequestDto project);
        Task<ApiResponse<ProjectReponseDto>> UpdateProjectAsync(string boardId, string projectId, UpdateProjectRequestDto projectUpdate);
        Task<ApiResponse<Project>> GetProjectByIdAsync(string projectId);
        Task<ApiResponse<PageResult<IEnumerable<Project>>>> GetProjectsByBoardIdAsync(string boardId, int perPage, int page);
        ApiResponse<string> DeleteAllProjects();
    }
}
