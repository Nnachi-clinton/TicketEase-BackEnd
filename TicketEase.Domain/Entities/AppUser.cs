using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace TicketEase.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Address { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public Manager Manager { get; set; }
        public string CloudinaryPublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
