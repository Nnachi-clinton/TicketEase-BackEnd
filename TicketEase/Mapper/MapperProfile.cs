using AutoMapper;
using TicketEase.Application.DTO;
using TicketEase.Domain.Entities;

namespace TicketEase.Mapper
{
    public class MapperProfile : Profile
    {
        protected MapperProfile()
        {
            CreateMap<BoardRequestDto, Board>();
            CreateMap<Board, BoardResponseDto>().ReverseMap();
        }
    }
}
