using TicketEase.Domain.Entities;

namespace TicketEase.Application.DTO
{
    public class BoardResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public string Description { get; set; }
        public ICollection<Domain.Entities.Project> projects { get; set; }
    }
}
