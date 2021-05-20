using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class RecoveryData
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public int RecoveryDataId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
