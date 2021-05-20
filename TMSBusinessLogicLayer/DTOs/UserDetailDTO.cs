
using System;
using System.Collections.Generic;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.DTOs
{
    public class UserDetailDTO { 
        public long PersonalDetailsId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int BranchId { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int RoleId { get; set; }
    //public ICollection<CredentialDTO> Credentials { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public Positions Position { get; set; }
 
    }
}
