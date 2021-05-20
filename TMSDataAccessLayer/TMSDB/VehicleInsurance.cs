using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class VehicleInsurance
    {
        public long VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateEffective { get; set; }
        public string IssuedBy { get; set; }
    }
}
