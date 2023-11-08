using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Application.DTO;
using TicketEase.Domain;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IBoardServices
    {
        Task<ApiResponse<BoardResponseDto>> AddBoardAsync(BoardRequestDto boardRequestDto);
        Task<ApiResponse<BoardResponseDto>> UpdateBoardAsync(string boardId, BoardRequestDto boardRequestDto);
    }
}
