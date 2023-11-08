using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.DTO
{
    public class BoardResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public string Description { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
