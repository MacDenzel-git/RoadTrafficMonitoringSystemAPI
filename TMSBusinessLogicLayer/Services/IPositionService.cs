 using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface IPositionService
    {
        Task<IEnumerable<Positions>> GetAllPosition();
    }
}
