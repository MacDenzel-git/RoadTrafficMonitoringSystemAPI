using System;
using System.Collections.Generic;
using System.Text;

namespace TMSBusinessLogicLayer.DTOs
{
    public class CrimeMantainanceDTO
    {
        public int CrimeChargeId { get; set; }
        public string CrimeName { get; set; }
        public decimal Charge { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
