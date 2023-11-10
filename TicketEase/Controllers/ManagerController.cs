using Microsoft.AspNetCore.Mvc;
using TicketEase.Application.DTO;
using TicketEase.Application.Interfaces.Services;

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
    }
}
