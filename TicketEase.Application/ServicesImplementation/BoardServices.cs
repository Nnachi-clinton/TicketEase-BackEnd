using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketEase.Application.DTO;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Domain;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.ServicesImplementation
{
    public class BoardServices : IBoardServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BoardServices> _logger;
        public BoardServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BoardServices> logger) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<BoardResponseDto>> AddBoardAsync(BoardRequestDto boardRequestDto)
        {
            ApiResponse<BoardResponseDto> response;

            try
            {   
                var existingBoard = _unitOfWork.BoardRepository.FindBoard(b => b.Name == boardRequestDto.Name).FirstOrDefault();
                if (existingBoard != null)
                {
                    response = new ApiResponse<BoardResponseDto>(false, 400, $"Board already exists.");
                    return response;
                }

                var board = _mapper.Map<Board>(boardRequestDto);
                _unitOfWork.BoardRepository.AddBoard(board);
                _unitOfWork.SaveChanges();
                
                var responseDto = _mapper.Map<BoardResponseDto>(board);
                response = new ApiResponse<BoardResponseDto>(true, $"Successfully added a board", 201, responseDto, new List<string>());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a board");
                var errorList = new List<string>();
                errorList.Add(ex.InnerException.ToString());
                response = new ApiResponse<BoardResponseDto>(true, "Error occurred while adding a board", 500, errorList);
                return response;
            }
        }



        public Task<ApiResponse<BoardResponseDto>> UpdateBoardAsync(string boardId, BoardRequestDto boardRequestDto)
        {
            //ApiResponse<BoardResponseDto> response;
            try
            {
                var existingBoard = _unitOfWork.BoardRepository.GetBoardById(boardId);
                if (existingBoard == null)
                {
                    return Task.FromResult(new ApiResponse<BoardResponseDto>(false, 400, $"Board not found."));
                    //return response;
                }

                var board = _mapper.Map(boardRequestDto, existingBoard);
                _unitOfWork.BoardRepository.UpdateBoard(existingBoard);
                _unitOfWork.SaveChanges();

                var responseDto = _mapper.Map<BoardResponseDto>(board);
                return Task.FromResult(new ApiResponse<BoardResponseDto>(true, $"Successfully added a board", 201, responseDto, new List<string>()));
                //return response;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while adding a board");
                var errorList = new List<string>();
                errorList.Add(ex.Message);
                return Task.FromResult(new ApiResponse<BoardResponseDto>(true, "Error occurred while adding a board", 500, null, errorList));
                //return response;

            }
        }
    }
}
