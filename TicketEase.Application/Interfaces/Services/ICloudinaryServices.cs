using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketEase.Application.Interfaces.Services
{
    public interface ICloudinaryServices
    {
        Task<string> UploadContactImage(string id, IFormFile file);
        // Task<User?> GetByIdAsync(string id);
       // Task SaveChangesAsync();

    }
}
