using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
using TMSBusinessLogicLayer.Repository;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services.Position
{
    public class PositionService : IPositionService
    {
        readonly TMSGenericRepository<Positions> _positionRepository;
        public PositionService(TMSGenericRepository<Positions> positionRepository)
        {
            _positionRepository = positionRepository;
        }
        public async Task<IEnumerable<Positions>> GetAllPosition()
        {
            return await _positionRepository.GetAll();
        }
    }
}
