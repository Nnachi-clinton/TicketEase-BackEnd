using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.DTO;
using TicketEase.Application.Interfaces.Services;

namespace TicketEase.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TicketController : ControllerBase
	{
		private readonly ITicketService _ticketService;

		public TicketController(ITicketService ticketService)
		{
			_ticketService = ticketService;
		}

        [HttpPost("add-ticket")]
        public IActionResult AddTicket([FromBody] TicketDto ticketDTO)
        {
            var response = _ticketService.AddTicket(ticketDTO);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("edit-ticket/{Id}")]
        public IActionResult EditTicket(string Id, [FromBody] UpdateTicketDto updatedTicketDTO)
        {
            var response = _ticketService.EditTicket(Id, updatedTicketDTO);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("user/{userId}")]
		public async Task<IActionResult> GetTicketsByUserId(string userId, int page, int perPage)
		{

			var result = await _ticketService.GetTicketByUserId(userId, page, perPage);
			return Ok(result);

		}

		[HttpGet("project/{projectId}")]
		public async Task<IActionResult> GetTicketsByProjectId(string projectId, int page, int perPage)
		{
			var result = await _ticketService.GetTicketByProjectId(projectId, page, perPage);
			return Ok(result);

		}
	}
}
