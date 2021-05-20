using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class DriverLicense
    {
        public string LicenseNumber { get; set; }
        public string Trn { get; set; }
        public DateTime FirstIssue { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CountryIssued { get; set; }
        public string LicenseCode { get; set; }
        public int VehicleRestriction { get; set; }
        public string DriverRestriction { get; set; }
        public int IssueNumber { get; set; }
        public long PersonId { get; set; }

        public PersonalDetails Person { get; set; }
    }
}
