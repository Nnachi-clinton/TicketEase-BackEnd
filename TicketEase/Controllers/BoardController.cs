using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.DTO;
using TicketEase.Application.Interfaces.Services;

namespace TicketEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardServices _boardServices;
        public BoardController(IBoardServices boardServices)
        {
            _boardServices = boardServices;
        }

        [HttpPost("AddBoard")]
        public async Task<IActionResult> AddBoard([FromBody] BoardRequestDto request)
        {
            return Ok(await _boardServices.AddBoardAsync(request));
        }

        [HttpPut("UpdateBoard/{boardId}")]
        public async Task<IActionResult> UpdateBoard(string boardId, [FromBody] BoardRequestDto request)
        {
            return Ok(await _boardServices.UpdateBoardAsync(boardId, request));
        }
    }
}
