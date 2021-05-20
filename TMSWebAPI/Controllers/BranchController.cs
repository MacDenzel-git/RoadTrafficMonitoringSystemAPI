using TMSBusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        readonly IBranchService _branchService;
        public BranchController(IBranchService service)
        {
            _branchService = service;
        }
        
        /// <summary>
        /// This API gets all branches available
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            var output = await _branchService.GetAllBranches();
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