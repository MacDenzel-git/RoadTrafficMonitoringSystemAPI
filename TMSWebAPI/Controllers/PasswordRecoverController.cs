using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Services;
using TMSBusinessLogicLayer.Services.PasswordRecover;
using TMSBusinessLogicLayer.Services.PasswordRecover.PasswordRecoverDTO;
 
using Microsoft.AspNetCore.Mvc;
using TMSBusinessLogicLayer;
using TMSDataAccessLayer.TMSDB;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Repository;

namespace TMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordRecoverController : ControllerBase
    {
        private readonly IPasswordRecover _passwordRecover;
        private readonly TMSGenericRepository<PersonalDetails> _personDetails;
        public PasswordRecoverController(IPasswordRecover passwordRecover, TMSGenericRepository<PersonalDetails> personDetails)
        {
            _passwordRecover = passwordRecover;
            _personDetails = personDetails;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(LoginViewModel loginViewModel)
        {

            //Get account using username
            try
            {
                var memberDetails = await _passwordRecover.GetCredentials(loginViewModel.Username);

                //Generate one time pin save to DB with date time -- Account RecoveryTable 
                var otp = _passwordRecover.RandomPassword();

                ///TO DO IF ALREADY EXIST DELETE REGERATE AND SEND EMAIL LOGIC
                var exist = await _passwordRecover.GetRecoveryInformation(loginViewModel.Username);
                if (exist != null)
                {
                    TimeSpan duration = DateTime.UtcNow.AddHours(2).Subtract(exist.CreatedDate); //get the difference between the times in minutes
                    int timeElapsed = Convert.ToInt32(duration.TotalMinutes);
                    Debug.WriteLine(duration);

                    //Add created date to recover info
                    if (timeElapsed >= 20) //OTP expires in 20 minutes.
                    {
                         
                        var output = await _passwordRecover.DeleteOTPRecord(loginViewModel.Username); //if expired delete it
                        await _passwordRecover.AddRecoveryInformation(memberDetails.Username, otp);
                        //Send Email of OTP 
                        await _passwordRecover.EmailMember(memberDetails.Username, otp,"Recover");
                        return Ok(new OutputHandler { IsErrorOccured = false, Message = "A One Time Pin has been sent to your email" });
                    }
                    else
                    {
                        OutputHandler result = new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = "One Time Password already exist for this account, Check your email or wait for 20 minutes and try again"
                        };
                        return Ok(result);
                    }
                }

                await _passwordRecover.AddRecoveryInformation(memberDetails.Username, otp);
                //Send Email of OTP 
                await _passwordRecover.EmailMember(memberDetails.Username, otp, "Recover");
                return Ok(new OutputHandler { IsErrorOccured = false, Message = "A One Time Pin has been sent to your email" });
            }
            catch (Exception)
            {

                return BadRequest(new LoginViewModel { IsErrorOccured = true, Message = "Something went Wrong, Contact Admin" });
            }
            //--Create Method for VErifying OTP
            //Login 
            //Change Password Access

        }

         
        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDTO loginViewModel)
        {
            var credentials = await _passwordRecover.GetCredentials(loginViewModel.Username);
            var recoveryInformation = await _passwordRecover.GetRecoveryInformation(credentials.Username);
            if (recoveryInformation != null)
            {

                if (recoveryInformation.Otp.Equals(loginViewModel.Otp))
                {
                    TimeSpan duration = DateTime.UtcNow.AddHours(2).Subtract(recoveryInformation.CreatedDate); //get the difference between the times in minutes
                    int timeElapsed = Convert.ToInt32(duration.TotalMinutes);
                    Debug.WriteLine(duration);

                    //Add created date to recover info
                    if (timeElapsed >= 20) //OTP expires in 20 minutes.
                    {
                        OutputHandler result = new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = "One Time Password has Expired, please do another Reset"
                        };
                        var output = await _passwordRecover.DeleteOTPRecord(loginViewModel.Username); //if expired delete it
                        return Ok(result);
                    }
                    else
                    {
                        //delete otp records to avoid having multiple records of the same account
                        //consider
                        var output = await _passwordRecover.DeleteOTPRecord(loginViewModel.Username);
                        if (output.IsErrorOccured)
                        {
                            return BadRequest(new OutputHandler { IsErrorOccured = output.IsErrorOccured, Message = output.Message });
                        }

                        var personalDetails = await _personDetails.GetItem(x => x.EmailAddress == loginViewModel.Username);
                        var MappedPersonalDetails = new AutoMapper<PersonalDetails, PersonalDetailsDTO>().MapToObject(personalDetails);

                        
                        return Ok(new OutputHandler { IsErrorOccured = false, Result = MappedPersonalDetails });
                       
                    }
                }
            }

            return Ok(new OutputHandler { IsErrorOccured = true, Message = "Wrong One time Password, Please try again or contact admin" });

        }


       
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(LoginViewModel loginViewModel)
        {
            if (loginViewModel.Password.Equals(loginViewModel.ConfirmPassword))
            {
                await _passwordRecover.UpdateCredentials(loginViewModel);
                return Ok();
            }
            else
            {
                return BadRequest(new LoginViewModel { IsErrorOccured = true, Message = "Password do not match" });
            }

        }

        
    }
}