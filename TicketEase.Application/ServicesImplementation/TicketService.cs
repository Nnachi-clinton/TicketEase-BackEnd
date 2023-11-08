using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.ServicesImplementation
{
	public class TicketService : ITicketService
	{
		private readonly IUnitOfWork _unitOfWork;

		public TicketService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<ApiResponse<PageResult<IEnumerable<Ticket>>>> GetTicketByProjectId(string projectId, int page, int perPage)
		{
			var tickets = await _unitOfWork.TicketRepository.GetTicketByProjectId(ticket => ticket.ProjectId == projectId);

			// Use the Pagination class to paginate the data
			var pagedTickets = await Pagination<Ticket>.GetPager(
			tickets,
			perPage,
			page,
			ticket => ticket.Title,
			ticket => ticket.Id.ToString());


			//return pagedTickets;
			return new ApiResponse<PageResult<IEnumerable<Ticket>>>(true, "Operation succesful", 200, null, new List<string>());
		}

		public async Task<ApiResponse<PageResult<IEnumerable<Ticket>>>> GetTicketByUserId(string userId, int page, int perPage)
		{
			var tickets = await _unitOfWork.TicketRepository.GetTicketByUserId(ticket => ticket.AppUserId == userId);

			// Use the Paginatioo paginate the dat
			var pagedTickets = await Pagination<Ticket>.GetPager(
				tickets,
				perPage,
				page,
				ticket => ticket.Title,
				ticket => ticket.Id.ToString());

			return new ApiResponse<PageResult<IEnumerable<Ticket>>> (true, "Operation succesful", 200, null, new List<string>());
			//{
			//	Status = "Success",
			//	Data = pagedTickets
			//};

			//return pagedTickets;
		}
	}
}
