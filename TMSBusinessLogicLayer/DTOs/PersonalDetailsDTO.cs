
using System;
using System.Collections.Generic;
using System.Text;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.DTOs
{
    public class PersonalDetailsDTO
    {
        public long PersonalDetailsId { get; set; }
        public string Name { get; set; }
        public DateTime Age { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int BranchId { get; set; }
        public int PositionId { get; set; }
        public int RoleId { get; set; }
  
        //public CredentialDTO Credentials { get; set; }
    }
}
