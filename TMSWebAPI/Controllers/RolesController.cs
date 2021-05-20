using TMSBusinessLogicLayer.Services.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        readonly IRoleService _service;
        public RolesController(IRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// This API 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var output = await _service.GetAllRoles();
            if (output == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(output);
            }
        }
    }
}