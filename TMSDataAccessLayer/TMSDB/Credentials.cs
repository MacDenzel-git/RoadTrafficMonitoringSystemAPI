using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class Credentials
    {
        public int CredentialId { get; set; }
        public long PersonalDetailsId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public PersonalDetails PersonalDetails { get; set; }
    }
}
