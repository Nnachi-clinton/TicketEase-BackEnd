using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.Interfaces.Services;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
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
    }
}
