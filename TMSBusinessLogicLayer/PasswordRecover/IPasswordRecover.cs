using TMSWebAPI.Controllers;
using TMSBusinessLogicLayer.Services.PasswordRecover.PasswordRecoverDTO;
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Services;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface IPasswordRecover
    {
        Task AddRecoveryInformation(string email, string otp);
        Task<RecoveryData> GetRecoveryInformation(string email);
        Task<OutputHandler> EmailMember(string email, string otp, string action);
        Task<OutputHandler> DeleteOTPRecord(string username);
        Task<Credentials> GetCredentials(string memberNumber);
         string  RandomPassword();
        Task<OutputHandler> UpdateCredentials(LoginViewModel loginDetails);
    }
}
