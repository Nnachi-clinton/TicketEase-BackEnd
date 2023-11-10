using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketEase.Application.DTO
{
    public class ManagerInfoCreateDto
    {
        public string BusinessEmail { get; set; }
        public string CompanyName { get; set; }
        public string ReasonToOnboard { get; set; }
        public string AdminEmail { get; set; }
    }
}
