using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class Crimes
    {
        public int CrimeChargeId { get; set; }
        public string CrimeName { get; set; }
        public decimal Charge { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
