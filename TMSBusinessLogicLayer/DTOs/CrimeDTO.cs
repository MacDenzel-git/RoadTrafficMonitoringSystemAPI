using System;
using System.Collections.Generic;
using System.Text;

namespace TMSBusinessLogicLayer.DTOs
{
    public class CrimeDTO
    {
        public int CrimeChargeId { get; set; }
        public string CrimeName { get; set; }
        public decimal CrimeCharge { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string LoggedInUser { get; set; }
        public DateTime DateCreated { get; set; }


    }
}
