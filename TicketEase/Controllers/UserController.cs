using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.DTO;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Domain;

namespace TicketEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserServices userServices, ILogger<UserController> logger)
        {
            _userServices = userServices;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var response = await _userServices.GetUserByIdAsync(userId);

            if (response.Succeeded)
            {
                return Ok(response.Data);
            }

            return StatusCode(response.StatusCode, new { errors = response.Errors });
        }

        [HttpGet("get-Users-By-Pagination")]
        public async Task<IActionResult> GetUsersByPagination(int page = 1, int perPage = 3)
        {
            var response = await _userServices.GetUsersByPaginationAsync(page, perPage);

            if (response.Succeeded)
            {
                return Ok(response.Data);
            }

            return StatusCode(response.StatusCode, new { errors = response.Errors });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            var updateResult = await _userServices.UpdateUserAsync(id, updateUserDto);

            if (updateResult.Succeeded)
            {
                return Ok(new ApiResponse<bool>(true, "User updated successfully.", 200, true, null));
            }

            _logger.LogError("User update failed: {Message}", updateResult.Message);
            return BadRequest(new ApiResponse<bool>(false, "Failed to update user.", 400, false, updateResult.Errors));
        }



    }
}

