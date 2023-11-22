using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketEase.Domain.Entities
{
    public class Manager
    {
        [Key]
        public string Id { get; set; }    
        public string CompanyName { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string State { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
		public string ImageUrl { get; set; }
     //   public ICollection<Manager> Managers { get; set; }
        public ICollection<AppUser> Users { get; set; }
        public ICollection<Board> Boards { get; set; }
    }
}