using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendHtmlEmailAsync(MailRequest request);
    }
}
