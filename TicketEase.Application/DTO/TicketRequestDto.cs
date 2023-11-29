using System;
using System.ComponentModel.DataAnnotations;
using TicketEase.Domain.Enums;

namespace TicketEase.Application.DTO
{
    public class TicketRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [EnumDataType(typeof(Status), ErrorMessage = "Invalid status.")]
        public Status Status { get; set; }

        [EnumDataType(typeof(Priority), ErrorMessage = "Invalid priority.")]
        public Priority Priority { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format for ResolvedAt.")]
        public DateTime? ResolvedAt { get; set; }

        public string AssignedTo { get; set; }
    }
}
