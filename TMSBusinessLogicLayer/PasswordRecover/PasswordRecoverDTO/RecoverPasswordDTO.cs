 
using System;
using System.Collections.Generic;
using System.Text;
using TMSBusinessLogicLayer.Services;

namespace TMSBusinessLogicLayer.Services.PasswordRecover.PasswordRecoverDTO
{
    public class RecoverPasswordDTO : OutputHandler
    {

        public string Username { get; set; }
        public string Otp { get; set; }
    }
}
