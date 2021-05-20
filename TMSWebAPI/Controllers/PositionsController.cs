using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        readonly IPositionService _service;
        public PositionsController(IPositionService service)
        {
            _service = service;
        }

        /// <summary>
        /// This api is used to get positions
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPositions")]
        public async Task<IActionResult> GetAllPositions()
        {
            var output = await _service.GetAllPosition();
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