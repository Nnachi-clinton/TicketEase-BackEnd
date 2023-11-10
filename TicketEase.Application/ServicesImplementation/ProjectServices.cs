using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketEase.Application.DTO.Project;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.ServicesImplementation
{
    public class ProjectServices : IProjectServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProjectServices> _logger;
        private readonly IMapper _mapper;

        public ProjectServices(IUnitOfWork unitOfWork, ILogger<ProjectServices> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProjectReponseDto>> CreateProjectAsync(string id, ProjectRequestDto project)
        {
            ApiResponse<ProjectReponseDto> response;
           
                var existingboard = _unitOfWork.BoardRepository.GetById(id);
                if (existingboard == null)
                {
                   response = new ApiResponse<ProjectReponseDto>(false, 404, $"Board with ID {id} not found.");
                    return response;
                }

                var existingProject = _unitOfWork.ProjectRepository.FindProject(p => p.BoardId == id && p.Title == project.Title).FirstOrDefault();

                if (existingProject != null)
                {
                  response = new ApiResponse<ProjectReponseDto>(false, 400, $"Project with the same name already exists in the board.");
                    return response;
                }
            try
            {

                var newProject = _mapper.Map<Project>(project);
                newProject.BoardId = id; // Set the board ID for the project

                _unitOfWork.ProjectRepository.AddProject(newProject);
                _unitOfWork.SaveChanges();

                var createdProject = _mapper.Map<ProjectReponseDto>(newProject);
               
                response = ApiResponse<ProjectReponseDto>.Success(createdProject, $"Successfully created a project in the board:{id}", 201);
                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while adding a board");
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                response = ApiResponse<ProjectReponseDto>.Failed(false, "Error occurred while creating a project in a board",500, new List<string> { ex.Message });
                return response;

            }
        }

        public async Task<ApiResponse<ProjectReponseDto>> UpdateProjectAsync(string boardId, string projectId, UpdateProjectRequestDto projectUpdate)
        {
            // Check if the board exists
            var existingBoard =  _unitOfWork.BoardRepository.GetById(boardId);
            if (existingBoard == null)
            {
                return new ApiResponse<ProjectReponseDto>(false, 404, $"Board with ID {boardId} not found.");

            }

            // Check if the project exists
            var existingProject =  _unitOfWork.ProjectRepository.GetById(projectId);
            if (existingProject == null)
            {
                return new ApiResponse<ProjectReponseDto>(false, 404, $"Board with ID {projectId} not found.");
            }

            try
            {
                // Update project properties based on projectUpdate
                existingProject.Title = projectUpdate.Title;
                existingProject.Description = projectUpdate.Description;

                _unitOfWork.ProjectRepository.UpdateProject(existingProject);
                 _unitOfWork.SaveChanges();

                var updatedProjectDto = _mapper.Map<ProjectReponseDto>(existingProject);
                return ApiResponse<ProjectReponseDto>.Success(updatedProjectDto, $"Successfully updated project with ID {projectId}", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a project");
                return ApiResponse<ProjectReponseDto>.Failed(false,"Error occurred while updating a project", 500, new List<string> { ex.Message });
            }




        }

        public async Task<ApiResponse<Project>> GetProjectByIdAsync(string projectId)
        {
            try
            {
                var project = _unitOfWork.ProjectRepository.GetProjectById(projectId);
                _logger.LogInformation("Project loaded successfully");

                return ApiResponse<Project>.Success(project, "Project loaded successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the project");

                return ApiResponse<Project>.Failed(false, "Error occurred while loading the project", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<PageResult<IEnumerable<Project>>>> GetProjectsByBoardIdAsync(string boardId, int perPage, int page)
        {
            try
            {
                var projects = _unitOfWork.ProjectRepository.GetAll();

                var boardProjects = projects.Where(project => project.BoardId == boardId).ToList();

                var paginationResponse = await Pagination<Project>.GetPager(boardProjects, perPage, page, p => p.Title, p => p.Id);

                return ApiResponse<PageResult<IEnumerable<Project>>>.Success(paginationResponse, "Successfully retrieved Projects", 200 );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the project");

                return ApiResponse<PageResult<IEnumerable<Project>>>.Failed(false, "Error occured whiile loading projects", 500, new List<string> {ex.Message});
            }
        }
    }
}
