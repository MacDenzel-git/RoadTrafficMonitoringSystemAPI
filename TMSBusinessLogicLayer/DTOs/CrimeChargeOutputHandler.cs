using System;
using System.Collections.Generic;
using System.Text;
using TMSBusinessLogicLayer.Services;

namespace TMSBusinessLogicLayer.DTOs
{
    public class CrimeChargeOutputHandler :OutputHandler 
    {
        public bool IsLicenseCrimeClean { get; set; }
        public bool IsRegistrationNumberCrimeClean { get; set; }
        public string LicenseMessage { get; set; }
        public string RegNumberMessage { get; set; }
    }
}
