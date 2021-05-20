using System;
using System.Collections.Generic;
using System.Text;

namespace TMSBusinessLogicLayer.DTOs
{
    public class Totals
    {
        public int CrimesCount { get; set; }
        public int Licenses { get; set; }
        public int VehiclesRegistered { get; set; }
        public int CrimesToday { get; set; }
    }
}
