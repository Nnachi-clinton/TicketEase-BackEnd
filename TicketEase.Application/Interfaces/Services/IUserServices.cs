using TicketEase.Application.DTO;
using TicketEase.Common.Utilities;
using TicketEase.Domain;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<ApiResponse<AppUserDto>> GetUserByIdAsync(string userId);
        Task<ApiResponse<PageResult<IEnumerable<AppUserDto>>>> GetUsersByPaginationAsync(int page, int perPage);
        Task<ApiResponse<bool>> UpdateUserAsync(string userId, UpdateUserDto userUpdateDto);
    }
}
