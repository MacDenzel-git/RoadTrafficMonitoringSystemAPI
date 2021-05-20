using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Services;

namespace TMSWebAPI.Controllers
{
    [Route("api/CrimeMaintanaince")]
    [ApiController]
    public class CrimeMantainanceController : ControllerBase
    {
        private readonly ICrimeMantainance _crime;
        public CrimeMantainanceController(ICrimeMantainance crime)
        {
            _crime = crime;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CrimeMantainanceDTO crime)
        {

            try
            {
                var output = await _crime.Create(crime);
                if (output.IsErrorOccured)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }




        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(CrimeMantainanceDTO crime)
        {
            try
            {
                var output = await _crime.Edit(crime);
                if (output.IsErrorOccured)
                {
                    return BadRequest(new OutputHandler { IsErrorOccured = true});
                }
                return Ok(output);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var output = await _crime.GetAll();
                if (output.IsErrorOccured)
                {
                    return BadRequest();
                }
                return Ok(output.Result);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }



        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int crime)
        {
            try
            {
                var output = await _crime.Delete(crime);
                if (output.IsErrorOccured)
                {
                    return BadRequest(output);
                }
                return Ok(output);
            }
            catch (Exception ex)
            {

                return BadRequest(new OutputHandler { IsErrorOccured = true , Message = "Something went wrong"});
            }



        }

        [HttpGet("GetCrime")]
        public async Task<IActionResult> GetCrime(int id)
        {
            try
            {
                var output = await _crime.GetById(id);
                return Ok(output);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }

}
