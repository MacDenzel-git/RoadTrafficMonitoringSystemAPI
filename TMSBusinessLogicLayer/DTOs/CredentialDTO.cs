using System;
using System.Collections.Generic;
using System.Text;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.DTOs
{
    public class CredentialDTO
    {
        public int CredentialId { get; set; }
        public long PersonalDetailsId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public PersonalDetailsDTO PersonalDetails { get; set; }
    }
}
