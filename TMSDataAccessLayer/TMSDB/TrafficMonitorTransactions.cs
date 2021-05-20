using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class TrafficMonitorTransactions
    {
        public long TransactionId { get; set; }
        public string CrimeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LicenseNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public decimal CrimeCharge { get; set; }
        public bool Paid { get; set; }
    }
}
