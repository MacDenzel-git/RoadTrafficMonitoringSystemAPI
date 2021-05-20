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
    [Route("api/[controller]")]
    [ApiController]
    public class CrimesController : ControllerBase
    {
        private readonly ICrimeService _crimeService;

        public CrimesController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }
        [HttpGet("GetCrimes")]
        public async Task<IActionResult> GetCrimes()
        {

            var output = await _crimeService.GetCrimes();
            if (output == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(output);
            }
        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions(DateTime start,DateTime end,bool isDateRangeProvided)
        {
             
            var output = await _crimeService.GetAllTransaction(start,end,isDateRangeProvided);
            if (output == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(output);
            }
        }

        [HttpPost("Charged")]
        public async Task<IActionResult> Charged(CrimeDTO crimeDTO)
        {

            var output = await _crimeService.Charged(crimeDTO);
            if (output == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(output);
            }
        }

        [HttpGet("GetRegistrationCharges")]
        public async Task<IActionResult> GetCrimesForRegNumber(string registrationNumber, string checkType)
        {
            var output = await _crimeService.GetRegistrationCharges(registrationNumber,checkType);
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