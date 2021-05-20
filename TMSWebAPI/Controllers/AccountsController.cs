 
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Services;

namespace TMSWebAPI.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly IAccountService _accountService;
             
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(CredentialDTO credetials)
        {
            var output = await _accountService.CreateCredentials(credetials);
            if (output.IsErrorOccured)
            {
                return BadRequest();
            }
            else
            {
                return Ok(new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = output.Message
                });
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var output = await _accountService.Login(request);

            if (output.IsErrorOccured)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(output.Result);
            }

        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDTO user)
        {
            var output = await _accountService.CreateUser(user);
            if (output.IsErrorOccured)
            {
                if (output.IsErrorKnown)
                {
                    return BadRequest(new OutputHandler
                    {
                        IsErrorOccured = true,
                        IsErrorKnown = output.IsErrorKnown,
                        Message = output.Message
                    });
                }
                else
                {
                    return BadRequest(new OutputHandler
                    {
                        IsErrorKnown = false
                    });
                }
                
            }
            else
            {
                return Ok(new OutputHandler
                {
                    IsErrorOccured = false,
                    Result = output.Result
                });
            }

        }

        [HttpPut("ChangeUserCredentialStatus")]
        public async Task<IActionResult> ChangeUserCredentialStatus(long PersonalDetailsId)
        {
            try
            {
                var output = await _accountService.ChangeUserCredentialStatus(PersonalDetailsId);
                return Ok(output);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
           
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var output = await _accountService.GetAllUsers();
                return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }


        }
        [HttpPut("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(long PersonalDetailsId)
        {
            try
            {
                var output = await _accountService.ChangeUserRole(PersonalDetailsId);
                return Ok(output);
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

       [HttpGet("Count")]
       public async Task<IActionResult> Count()
        {
            try
            {
                var totals = await _accountService.Count();
                return Ok(totals);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            var output = await _accountService.UpdateUser(user);
            if (output.IsErrorOccured)
            {
                if (output.IsErrorKnown)
                {
                    return BadRequest(new OutputHandler
                    {
                        IsErrorKnown = output.IsErrorKnown,
                        Message = output.Message,
                        IsErrorOccured = true
                    });
                }
                else
                {
                    return BadRequest(new OutputHandler
                    {
                        IsErrorKnown = false,
                        Message = output.Message,
                        IsErrorOccured = true

                    });
                }

            }
            else
            {
                return Ok(new OutputHandler
                {
                    IsErrorOccured = false,
                    Result = output.Result
                });
            }

        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(long personId)
        {
            try
            {
                var output = await _accountService.GetUserById(personId);
                return Ok(output);
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteUserById(long personId)
        {
            try
            {
                var output = await _accountService.DeleteUserById(personId);
                if (output.IsErrorOccured)
                {
                    return BadRequest();
                }
                return Ok(output);
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }
    }
}