using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketEase.Application.DTO;
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
        private readonly IMapper _mapper;
        private readonly ILogger<TicketService> _logger;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TicketService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public ApiResponse<bool> AddTicket(TicketDto ticketDTO)
        {
            try
            {
                var ticketEntity = _mapper.Map<Ticket>(ticketDTO);
                ticketEntity.TicketReference = TicketHelper.GenerateTicketReference();
                _unitOfWork.TicketRepository.AddTicket(ticketEntity);
                _unitOfWork.SaveChanges();
                _logger.LogInformation("Ticket added successfully");
                return ApiResponse<bool>.Success(true, "Ticket added successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a ticket");
                return ApiResponse<bool>.Failed(new List<string> { "Error: " + ex.Message });
            }
        }


        public ApiResponse<bool> EditTicket(string ticketId, UpdateTicketDto updatedTicketDTO)
        {
            try
            {
                var existingTicket = _unitOfWork.TicketRepository.GetTicketById(ticketId);

                if (existingTicket == null)
                {
                    _logger.LogWarning("Ticket not found while trying to edit");
                    return ApiResponse<bool>.Failed(new List<string> { "Ticket not found" });
                }

                _mapper.Map(updatedTicketDTO, existingTicket);

                _unitOfWork.TicketRepository.UpdateTicket(existingTicket);
                _unitOfWork.SaveChanges();

                _logger.LogInformation("Ticket updated successfully");
                return ApiResponse<bool>.Success(true, "Ticket updated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a ticket");
                return ApiResponse<bool>.Failed(new List<string> { "Error: " + ex.Message });
            }
        }
        public async Task<ApiResponse<PageResult<IEnumerable<Ticket>>>> GetTicketByProjectId(string projectId, int page, int perPage)
		{
			var tickets = await _unitOfWork.TicketRepository.GetTicketByProjectId(ticket => ticket.ProjectId == projectId);
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
