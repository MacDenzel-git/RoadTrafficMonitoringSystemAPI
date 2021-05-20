using System;
using System.Collections.Generic;
using System.Text;
using TMSDataAccessLayer.TMSDB;

namespace TMSDataAccessLayer
{
   public class LicenseViewModel
    {
        public long PersonId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string CreatedBy { get; set; }
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
    }
}
