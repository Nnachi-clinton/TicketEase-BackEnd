using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TicketEase.Application.DTO;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.ServicesImplementation
{
    public class ManagerServices : IManagerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ManagerServices> _logger;
        public ManagerServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ManagerServices> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public Task<ApiResponse<EditManagerDto>> EditManager(string userId, EditManagerDto editManagerDto)
        {
            try
            {
                var existingManager = _unitOfWork.ManagerRepository.GetManagerById(userId);
                if (existingManager == null)
                {
                    _logger.LogWarning("Manager with such Id does not exist");
                    return Task.FromResult(new ApiResponse<EditManagerDto>(false, StatusCodes.Status400BadRequest, $"Manager not found."));
                }
               var manager = _mapper.Map(editManagerDto, existingManager);
                _unitOfWork.ManagerRepository.UpdateManager(existingManager);
                _unitOfWork.SaveChanges();
                var responseDto = _mapper.Map<EditManagerDto>(manager);
                _logger.LogInformation("Manager updated successfully");
                return Task.FromResult(new ApiResponse<EditManagerDto>(true, $"Successfully updated a Manager", 201, responseDto, new List<string>()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a Manager");
                var errorList = new List<string>();
                return Task.FromResult(new ApiResponse<EditManagerDto>(true, "Error occurred while adding a Manager", StatusCodes.Status400BadRequest, null, errorList));
            }
        }
        public async Task<ApiResponse<PageResult<IEnumerable<Manager>>>> GetAllManagerByPagination(int page, int perPage)
        {
            try
            {    
                var managers = _unitOfWork.ManagerRepository.GetAll();

                var pagedManagers = await Pagination<Manager>.GetPager(
                    managers,
                    perPage,
                    page,
                    manager => manager.CompanyName,
                    manager => manager.BusinessEmail);
                var response = new ApiResponse<PageResult<IEnumerable<Manager>>>(
                    true,
                    "Operation successful",
                    StatusCodes.Status200OK,
                    new PageResult<IEnumerable<Manager>>
                    {
                        Data = pagedManagers.Data.ToList(),
                        TotalPageCount = pagedManagers.TotalPageCount,
                        CurrentPage = pagedManagers.CurrentPage,
                        PerPage = perPage,
                        TotalCount = pagedManagers.TotalCount
                    },
                    new List<string>());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving paged managers");
                return ApiResponse<PageResult<IEnumerable<Manager>>>.Failed(new List<string> { "Error: " + ex.Message });
            }
        }
        public ApiResponse<EditManagerDto> GetManagerById(string userId)
        {
            try
            {
                var existingManager = _unitOfWork.ManagerRepository.GetManagerById(userId);
                if (existingManager == null)
                {
                    _logger.LogWarning("Manager with found ");
                    return ApiResponse<EditManagerDto>.Failed(new List<string> { "Manager not found" });
                }
                var Manager = _mapper.Map<EditManagerDto>(existingManager);
                _logger.LogInformation("Manager retrieved successfully");
                return ApiResponse<EditManagerDto>.Success(Manager, "Manager retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {   
                _logger.LogError(ex, "Error occurred while retrieving a Manager");
                return ApiResponse<EditManagerDto>.Failed(new List<string> { "Error: " + ex.Message });
            }
        }       
    }
}
