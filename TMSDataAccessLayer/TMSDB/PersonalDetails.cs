using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class PersonalDetails
    {
        public PersonalDetails()
        {
            Credentials = new HashSet<Credentials>();
        }

        public long PersonalDetailsId { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int BranchId { get; set; }
        public int PositionId { get; set; }
        public int RoleId { get; set; }
        public string LastName { get; set; }

        public Positions Position { get; set; }
        public Roles Role { get; set; }
        public DriverLicense DriverLicense { get; set; }
        public ICollection<Credentials> Credentials { get; set; }
    }
}
