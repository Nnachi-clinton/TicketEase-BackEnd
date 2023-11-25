using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketEase.Application.DTO;
using TicketEase.Application.DTO.Manager;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Common.Utilities;
using TicketEase.Domain;
using TicketEase.Domain.Entities;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace TicketEase.Application.ServicesImplementation
{
	public class ManagerServices : IManagerServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<ManagerServices> _logger;
		private readonly IEmailServices _emailServices;
		private readonly UserManager<AppUser> _userManager;
		private readonly IConfiguration _config;
		IAuthenticationService _authenticationService;
		public ManagerServices(IAuthenticationService authenticationService, UserManager<AppUser> userManager, IConfiguration config, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ManagerServices> logger, IEmailServices emailServices)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_emailServices = emailServices;
			_config = config;
			_userManager = userManager;
			_authenticationService = authenticationService;
		}

		public async Task<ApiResponse<ManagerResponseDto>> CreateManager(ManagerInfoCreateDto managerCreateDto)
		{
			var user = _unitOfWork.ManagerRepository.FindManager(x => x.BusinessEmail == managerCreateDto.BusinessEmail);
			if (user.Count > 0)
			{
				return new ApiResponse<ManagerResponseDto> (false,StatusCodes.Status400BadRequest,"Manager with this email already exist.");
			}

			string generatedPassword = PasswordGenerator.GeneratePassword(managerCreateDto.BusinessEmail, managerCreateDto.CompanyName);
			managerCreateDto.Password = generatedPassword;
			string emailBody = $"Welcome to TicketEase. An account has been created for you with the following details<br>Email: {managerCreateDto.BusinessEmail}, <br>Password: {generatedPassword}";

			var manager = new Manager()
			{
				BusinessEmail = managerCreateDto.BusinessEmail,
				CompanyDescription = managerCreateDto.CompanyDescription,
				CompanyName = managerCreateDto.CompanyName
			};

			_unitOfWork.ManagerRepository.AddManager(manager);
			_unitOfWork.SaveChanges();

			var userCreateDto = new AppUserCreateDto()
			{
				ManagerId = manager.Id,
				Email = managerCreateDto.BusinessEmail,
				Password = generatedPassword
			};

			var response = await _authenticationService.RegisterManagerAsync(userCreateDto);
			if (response.Succeeded)
			{
				try
				{
					var email = new MailRequest()
					{
						Subject = "Welcome to TicketEase",
						ToEmail = managerCreateDto.BusinessEmail,
						Body = emailBody
					};
					var managerResponse = _mapper.Map<ManagerResponseDto>(manager);
					await _emailServices.SendHtmlEmailAsync(email);
					return new ApiResponse<ManagerResponseDto>(true, response.Message, StatusCodes.Status200OK, managerResponse, new List<string>()); 
				}
				catch (Exception ex)
				{
					return new ApiResponse<ManagerResponseDto>(false, response.Message, StatusCodes.Status500InternalServerError, new List<string>() { ex.InnerException.ToString()});
				}
			}
			else
			{
				_unitOfWork.ManagerRepository.Delete(manager);
				_unitOfWork.SaveChanges();
				return new ApiResponse<ManagerResponseDto>(false, StatusCodes.Status500InternalServerError, response.Message);
			}

		}


		public Task<ApiResponse<EditManagerDto>> EditManager(string userId, EditManagerDto editManagerDto)
		{
			try
			{
				var existingManager = _unitOfWork.ManagerRepository.GetManagerById(userId);
				if (existingManager == null)
				{
					_logger.LogWarning("Manager with such Id does not exist");
					return Task.FromResult(new ApiResponse<EditManagerDto>(false, StatusCodes.Status400BadRequest, $"Manager not found."));
				}
				var manager = _mapper.Map(editManagerDto, existingManager);
				_unitOfWork.ManagerRepository.UpdateManager(existingManager);
				_unitOfWork.SaveChanges();
				var responseDto = _mapper.Map<EditManagerDto>(manager);
				_logger.LogInformation("Manager updated successfully");
				return Task.FromResult(new ApiResponse<EditManagerDto>(true, $"Successfully updated a Manager", 201, responseDto, new List<string>()));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while editing a Manager");
				var errorList = new List<string>();
				return Task.FromResult(new ApiResponse<EditManagerDto>(false, "Error occurred while adding a Manager", StatusCodes.Status400BadRequest, null, errorList));
			}
		}
		public async Task<ApiResponse<PageResult<IEnumerable<Manager>>>> GetAllManagerByPagination(int page, int perPage)
		{
			try
			{
				var managers = _unitOfWork.ManagerRepository.GetAll();

				var pagedManagers = await Pagination<Manager>.GetPager(
					managers,
					perPage,
					page,
					manager => manager.CompanyName,
					manager => manager.BusinessEmail);

				var response = new ApiResponse<PageResult<IEnumerable<Manager>>>(true, "Operation successful", StatusCodes.Status200OK,

					new PageResult<IEnumerable<Manager>>
					{
						Data = pagedManagers.Data.ToList(),
						TotalPageCount = pagedManagers.TotalPageCount,
						CurrentPage = pagedManagers.CurrentPage,
						PerPage = perPage,
						TotalCount = pagedManagers.TotalCount
					},

					new List<string>());
				return response;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving paged managers");
				return ApiResponse<PageResult<IEnumerable<Manager>>>.Failed(new List<string> { "Error: " + ex.Message });
			}
		}
		public ApiResponse<EditManagerDto> GetManagerById(string userId)
		{
			try
			{
				var existingManager = _unitOfWork.ManagerRepository.GetManagerById(userId);
				if (existingManager == null)
				{
					_logger.LogWarning("Manager with found ");
					return ApiResponse<EditManagerDto>.Failed(new List<string> { "Manager not found" });
				}
				var Manager = _mapper.Map<EditManagerDto>(existingManager);
				_logger.LogInformation("Manager retrieved successfully");
				return ApiResponse<EditManagerDto>.Success(Manager, "Manager retrieved successfully", StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving a Manager");
				return ApiResponse<EditManagerDto>.Failed(new List<string> { "Error: " + ex.Message });
			}
		}

		public async Task<ApiResponse<string>> ConfirmEmailAsync(string email, string token)
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
			{
				return new ApiResponse<string>(false, StatusCodes.Status404NotFound, "User not found.");
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);

			if (result.Succeeded)
			{
				return new ApiResponse<string>(true, StatusCodes.Status200OK, "Email confirmation successful.");
			}

			// Handle confirmation failure
			return new ApiResponse<string>(false, StatusCodes.Status400BadRequest, "Email confirmation failed.");
		}


		public async Task<ApiResponse<bool>> UpdateManagerProfileAsync(string managerId, UpdateManagerDto updateManagerDto)
		{
			try
			{
				var manager = _unitOfWork.ManagerRepository.GetManagerById(managerId);
				if (manager == null)
				{
					return ApiResponse<bool>.Failed(false, "Manager not found.", StatusCodes.Status404NotFound, new List<string> { "Manager not found." });
				}
				//var managerr = _mapper.Map<Manager>(updateManagerDto);
				manager.UpdatedDate = DateTime.UtcNow;
				manager.BusinessEmail = updateManagerDto.BusinessEmail;
				manager.State = updateManagerDto.State;
				manager.BusinessPhone = updateManagerDto.BusinessPhone;
				manager.CompanyAddress = updateManagerDto.CompanyAddress;
				manager.CompanyName = updateManagerDto.CompanyName;

				_unitOfWork.ManagerRepository.UpdateManager(manager);

				_unitOfWork.SaveChanges();
				return ApiResponse<bool>.Success(true, "Manager updated successfully.", StatusCodes.Status200OK);

			}
			catch (Exception ex)
			{
				return ApiResponse<bool>.Failed(false, "Some error occurred.", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
			}
		}


		public async Task<ApiResponse<bool>> SendManagerInformationToAdminAsync(ManagerInfoCreateDto managerInfoCreateDto)
		{
			try
			{
				managerInfoCreateDto.AdminEmail = "ilodibeonyedikachisom@gmail.com";
				var mailRequest = new MailRequest
				{
					ToEmail = managerInfoCreateDto.AdminEmail,
					Subject = "Manager Information",
					Body = $"Business Email: {managerInfoCreateDto.BusinessEmail}\n" +
						   $"Company Name: {managerInfoCreateDto.CompanyName}\n" +
						   $"Reason to Onboard: {managerInfoCreateDto.CompanyDescription}"
				};
				return ApiResponse<bool>.Success(true, "Manager information sent to admin successfully", 200);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "An error occurred while sending manager information to admin");
				return ApiResponse<bool>.Failed(new List<string> { "Error: " + ex.Message });
			}

		}


		private string GenerateJwtToken(Manager contact, string roles)
		{
			var jwtSettings = _config.GetSection("JwtSettings:Secret").Value;
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
					//new Claim(JwtRegisteredClaimNames.Sub, contact.UserName),
					//new Claim(JwtRegisteredClaimNames.Email, contact.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(ClaimTypes.Role, roles)
				};

			var token = new JwtSecurityToken(
				issuer: null,
				audience: null,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JwtSettings:AccessTokenExpiration").Value)),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<ApiResponse<string>> ForgotPasswordAsync(string email)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(email);

				if (user == null)
				{
					return new ApiResponse<string>(false, "User not found or email not confirmed.", StatusCodes.Status404NotFound, null, new List<string>());
				}
				string token = await _userManager.GeneratePasswordResetTokenAsync(user);

				var resetPasswordUrl = "https://localhost:7068/reset-password?email=" + Uri.EscapeDataString(email) + "&token=" + Uri.EscapeDataString(token);

				var mailRequest = new MailRequest
				{
					ToEmail = email,
					Subject = "Password Reset Instructions",
					Body = $"Please reset your password by clicking <a href='{resetPasswordUrl}'>here</a>."
				};
				await _emailServices.SendHtmlEmailAsync(mailRequest);

				return new ApiResponse<string>(true, "Password reset email sent successfully.", 200, null, new List<string>());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while resolving password change");
				var errorList = new List<string>();
				errorList.Add(ex.Message);
				return new ApiResponse<string>(true, "Error occurred while resolving password change", 500, null, errorList);
			}
		}

		public async Task<ApiResponse<string>> ResetPasswordAsync(string email, string token, string newPassword)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(email);

				if (user == null)
				{
					return new ApiResponse<string>(false, "User not found.", 404, null, new List<string>());
				}
				var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

				if (result.Succeeded)
				{
					return new ApiResponse<string>(true, "Password reset successful.", 200, null, new List<string>());
				}
				else
				{
					return new ApiResponse<string>(false, "Password reset failed.", 400, null, result.Errors.Select(error => error.Description).ToList());
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while resetting password");
				var errorList = new List<string> { ex.Message };
				return new ApiResponse<string>(true, "Error occurred while resetting password", 500, null, errorList);
			}
		}

		public async Task<ApiResponse<string>> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
		{
			try
			{
				var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

				if (result.Succeeded)
				{
					return new ApiResponse<string>(true, "Password changed successfully.", 200, null, new List<string>());
				}
				else
				{
					return new ApiResponse<string>(false, "Password change failed.", 400, null, result.Errors.Select(error => error.Description).ToList());
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while changing password");
				var errorList = new List<string> { ex.Message };
				return new ApiResponse<string>(true, "Error occurred while changing password", 500, null, errorList);
			}
		}

		public string DeactivateManager(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return "Manager Id must be provided";
			}

			var manager = _unitOfWork.UserRepository.GetUserById(id);

			if (manager != null)
			{
				manager.IsActive = false;
				_unitOfWork.UserRepository.UpdateUser(manager);
				_unitOfWork.SaveChanges();// Save changes to deactivate

			}
			return $"Manager with Id {id} has been deactivated successfully";

		}


		public string ActivateManager(string id)
		{
			var manager = _unitOfWork.UserRepository.GetUserById(id);
			if (manager != null)
			{
				manager.IsActive = true;
				_unitOfWork.UserRepository.UpdateUser(manager);
				_unitOfWork.SaveChanges();// Save changes to deactivate
				return $"Manager with Id {id} has been activated successfully";
			}
			else
			{
				return "Manager not found";
			}


		}
		public ApiResponse<string> ExtractUserIdFromToken(string authToken)
		{
			try
			{
				var token = authToken.Replace("Bearer ", "");

				var handler = new JwtSecurityTokenHandler();
				var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

				var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;

				if (string.IsNullOrWhiteSpace(userId))
				{
					return new ApiResponse<string>(false, "Invalid or expired token.", 401, null, new List<string>());
				}

				return new ApiResponse<string>(true, "User ID extracted successfully.", 200, userId, new List<string>());
			}
			catch (Exception ex)
			{
				return new ApiResponse<string>(false, "Error extracting user ID from token.", 500, null, new List<string> { ex.Message });
			}
		}

	}
}
