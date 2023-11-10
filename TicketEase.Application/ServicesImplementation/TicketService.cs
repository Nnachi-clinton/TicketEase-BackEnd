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

        public ApiResponse<TicketResponseDto> AddTicket(string userId, string ProjectId, TicketRequestDto ticketDTO)
        {
            ApiResponse<TicketResponseDto> response;
            try
            {
                var ticketEntity = _mapper.Map<Ticket>(ticketDTO);
                ticketEntity.TicketReference = TicketHelper.GenerateTicketReference();
                ticketEntity.AppUserId = userId;
                ticketEntity.ProjectId = ProjectId;

                _unitOfWork.TicketRepository.AddTicket(ticketEntity);
                _unitOfWork.SaveChanges();

                var responseDto = _mapper.Map<TicketResponseDto>(ticketEntity);
                response = new ApiResponse<TicketResponseDto>(true, $"Successfully added a ticket", 201, responseDto, new List<string>());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a ticket");
                return ApiResponse<TicketResponseDto>.Failed(new List<string> { "Error: " + ex.Message });
            }
        }

        public ApiResponse<TicketResponseDto> EditTicket(string ticketId, UpdateTicketRequestDto updatedTicketDTO)
        {
            ApiResponse<TicketResponseDto> response;
            try
            {
                var existingTicket = _unitOfWork.TicketRepository.GetTicketById(ticketId);

                if (existingTicket == null)
                {
                    _logger.LogWarning("Ticket not found while trying to edit");
                    return ApiResponse<TicketResponseDto>.Failed(new List<string> { "Ticket not found" });
                }

                _mapper.Map(updatedTicketDTO, existingTicket);

                _unitOfWork.TicketRepository.UpdateTicket(existingTicket);
                _unitOfWork.SaveChanges();

                var responseDto = _mapper.Map<TicketResponseDto>(existingTicket);
                response = new ApiResponse<TicketResponseDto>(true, $"Ticket updated successfully", 200, responseDto, new List<string>());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a ticket");
                return ApiResponse<TicketResponseDto>.Failed(new List<string> { "Error: " + ex.Message });
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
