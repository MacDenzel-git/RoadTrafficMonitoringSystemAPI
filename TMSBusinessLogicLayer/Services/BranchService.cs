using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Repository;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public class BranchService : IBranchService
    {
        readonly TMSGenericRepository<Branch> _branchRepository;
        public BranchService(TMSGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        //should always return OutputHandler

        public async Task<IEnumerable<Branch>> GetAllBranches()
        {
            return await _branchRepository.GetAll();
        }
    }
}
