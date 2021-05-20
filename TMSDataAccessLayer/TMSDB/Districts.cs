using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class Districts
    {
        public Districts()
        {
            Branch = new HashSet<Branch>();
        }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CountryId { get; set; }

        public ICollection<Branch> Branch { get; set; }
    }
}
