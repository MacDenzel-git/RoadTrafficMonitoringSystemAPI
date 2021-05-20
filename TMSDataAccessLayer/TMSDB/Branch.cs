using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public long DistrictId { get; set; }

        public Districts District { get; set; }
    }
}
