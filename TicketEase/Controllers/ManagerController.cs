using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.DTO;
using TicketEase.Application.DTO.Manager;
using TicketEase.Application.Interfaces.Services;
using TicketEase.Application.ServicesImplementation;
using TicketEase.Domain;

namespace TicketEase.Controllers
{
    [Route("api/managers")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerServices _managerService;
        

        public ManagerController(IManagerServices managerService)
        {
            _managerService = managerService;
            
        }

        [HttpPost("AddManager")]
        public async Task<IActionResult> CreateManager([FromBody] ManagerInfoCreateDto managerInfoCreateDto)
        {
            return Ok(await _managerService.CreateManager(managerInfoCreateDto));
        }


        [HttpGet("GetById")]
        public IActionResult GetManagersById(string id)
        {
            var response = _managerService.GetManagerById(id);
            return Ok(response);       
        }
        [HttpPut("Edit")]
        public IActionResult EditManager(string id, EditManagerDto managerDTO)
        {          
            var response = _managerService.EditManager(id, managerDTO);
            return Ok(response);            
        }
        [HttpGet("GetAll")]
        public IActionResult GetAllManage(int page, int perPage)
        {          
            var response = _managerService.GetAllManagerByPagination(page, perPage);
            return Ok(response);           
        }

        [HttpPost("sendManagerInformation")]
        public async Task<IActionResult> SendManagerInformation(ManagerInfoCreateDto managerInfoCreateDto)
        {    

                var response = await _managerService.SendManagerInformationToAdminAsync(managerInfoCreateDto);

                return Ok(response);            
        }

        [HttpPut("updateManager/{managerId}")]
        public async Task<IActionResult> UpdateManagerProfile(string managerId, [FromForm] UpdateManagerDto updateManagerDto)
        {
            var result = await _managerService.UpdateManagerProfileAsync(managerId, updateManagerDto);
            return Ok(new ApiResponse<bool>(true, "User updated successfully.", 200, true, null));              

           
        }
    }
}
