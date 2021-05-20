using System;
using System.Collections.Generic;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class Positions
    {
        public Positions()
        {
            PersonalDetails = new HashSet<PersonalDetails>();
        }

        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string Abbreviation { get; set; }

        public ICollection<PersonalDetails> PersonalDetails { get; set; }
    }
}
