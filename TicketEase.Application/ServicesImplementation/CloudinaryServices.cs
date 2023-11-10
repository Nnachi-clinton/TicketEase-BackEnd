using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.ServicesImplementation
{
    public class CloudinaryServices :  ICloudinaryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CloudinaryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AppUser?> GetByIdAsync(string id)
        {
            return  _unitOfWork.UserRepository.GetUserById(id);
        }




        public async Task<string> UploadContactImage(string Id, IFormFile file)
        {
            var user = await GetByIdAsync(Id);

            if (user == null)
            {
                return "User not found";
            }

            var cloudinary = new Cloudinary(new Account(
                "dlpryp6af",
                "969623236923961",
                "QL5lf-M_syJrxGJdJzbu2oRMAZA"
            ));

            var upload = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await cloudinary.UploadAsync(upload);

            // Update the user's ImageUrl property

            user.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;

            // Save the updated user entity to the database
            _unitOfWork.UserRepository.Update(user);

            try
            {
                _unitOfWork.SaveChanges();
               return user.ImageUrl;
                //return "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return "Database update error occurred";
            }
        }


    }
}
