using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMSBusinessLogicLayer.Services;
using TMSDataAccessLayer.TMSDB;

namespace TMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleInsurancesController : ControllerBase
    {
        private readonly IVehicleInsuranceService _context;

        public VehicleInsurancesController(IVehicleInsuranceService context)
        {
            _context = context;
        }

        // GET: api/VehicleInsurances
        [HttpGet("GetAllVehicleInsurance")]
        public async Task<IEnumerable<VehicleInsurance>> GetVehicleInsurance()
        {
            var output =
                await _context.GetAllVehicles();
            return output;
        }

        // GET: api/VehicleInsurances/5
        [HttpGet("GetVehicleInsurance")]
        public async Task<IActionResult> GetVehicleInsurance( long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleInsurance = await _context.GetVehicle(id);

            if (vehicleInsurance == null)
            {
                return NotFound();
            }

            return Ok(vehicleInsurance);
        }

        // PUT: api/VehicleInsurances/5
        [HttpPut("UpdateVehicleInsurance")]
        public async Task<IActionResult> PutVehicleInsurance( VehicleInsurance vehicleInsurance)
        {
            try
            {
                var updatedVehicleInsurance = vehicleInsurance; 
                var outputHandler = await _context.UpdateVehicle(updatedVehicleInsurance);
                return Ok(outputHandler);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/VehicleInsurances
        [HttpPost("CreateVehicleInsurance")]
        public async Task<IActionResult> PostVehicleInsurance([FromBody] VehicleInsurance vehicleInsurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.CreateVehicle(vehicleInsurance);
            
            return CreatedAtAction("GetVehicleInsurance", new { id = vehicleInsurance.VehicleId }, vehicleInsurance);
        }

        // DELETE: api/VehicleInsurances/5
        [HttpDelete("DeleteVehicleInsurance")]
        public async Task<IActionResult> DeleteVehicleInsurance(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleInsurance = await _context.GetVehicle(id);
            if (vehicleInsurance == null)
            {
                return NotFound();
            }

            await _context.DeleteVehicle(vehicleInsurance);
            
            return Ok(vehicleInsurance);
        }

     
    }
}