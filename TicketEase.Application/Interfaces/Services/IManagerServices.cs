using System;
using TicketEase.Application.DTO;
using TicketEase.Application.DTO.Manager;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IManagerServices
    {
		string DeactivateManager(string id);
        string ActivateManager(string id);
		Task<ApiResponse<ManagerResponseDto>> CreateManager(ManagerInfoCreateDto managerCreateDto);
		Task<ApiResponse<EditManagerDto>> EditManager(string userId, EditManagerDto managerDto);
        ApiResponse<EditManagerDto> GetManagerById(string userId);
        Task<ApiResponse<PageResult<IEnumerable<Manager>>>> GetAllManagerByPagination(int page, int perPage);
        Task<ApiResponse<bool>> SendManagerInformationToAdminAsync(ManagerInfoCreateDto managerDto);
        Task<ApiResponse<bool>> UpdateManagerProfileAsync(string managerId, UpdateManagerDto updateManagerDto);
    }
}
