using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class Roles
    {
        public Roles()
        {
            PersonalDetails = new HashSet<PersonalDetails>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<PersonalDetails> PersonalDetails { get; set; }
    }
}
