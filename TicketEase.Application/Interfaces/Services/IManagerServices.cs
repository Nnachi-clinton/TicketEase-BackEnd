using TicketEase.Application.DTO;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IManagerServices
    {
        Task<ApiResponse<EditManagerDto>> EditManager(string userId, EditManagerDto managerDto);
        ApiResponse<EditManagerDto> GetManagerById(string userId);
        Task<ApiResponse<PageResult<IEnumerable<Manager>>>> GetAllManagerByPagination(int page, int perPage);    
    }
}
