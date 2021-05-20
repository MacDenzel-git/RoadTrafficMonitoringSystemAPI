using System;

namespace TMSBusinessLogicLayer.DTOs
{
    public class DriverLicenseDTO
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
    }
}
