using System;
using System.Collections.Generic;
using System.Text;

namespace TMSBusinessLogicLayer.DTOs
{
   public class UserDTO
    {
        public int PersonalDetailsId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int BranchId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CredentialDTO CredetialDTO { get; set; }
        public int PositionId { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
    }
}
