using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TicketEase.Application.DTO.Manager
{
    public class ManagerInfoCreateDto
    {
        public string BusinessEmail { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }

        [JsonIgnore]
        public string AdminEmail { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
