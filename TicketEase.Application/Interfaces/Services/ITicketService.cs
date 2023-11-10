using TicketEase.Application.DTO;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Services
{
	public interface ITicketService
	{
		Task<ApiResponse<PageResult<IEnumerable<Ticket>>>> GetTicketByUserId(string userId, int page, int perPage);
		Task<ApiResponse<PageResult<IEnumerable<Ticket>>>> GetTicketByProjectId(string projectId, int page, int perPage);
        ApiResponse<TicketResponseDto> AddTicket(string userId, string ProjectId, TicketRequestDto ticketDTO);
        ApiResponse<TicketResponseDto> EditTicket(string ticketId, UpdateTicketRequestDto updatedTicketDTO);
    }
}
