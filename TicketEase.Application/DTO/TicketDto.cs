using TicketEase.Domain.Enums;

namespace TicketEase.Application.DTO
{
    public class TicketDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime ResolvedAt { get; set; }
        public string AssignedTo { get; set; }
        public string AppUserId { get; set; }
        public string ProjectId { get; set; }
    }
}
