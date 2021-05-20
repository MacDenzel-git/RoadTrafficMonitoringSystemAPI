using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
using TMSBusinessLogicLayer.Repository;

namespace TMSBusinessLogicLayer.Services.Roles
{
    public class RoleService : IRoleService
    {
        readonly TMSGenericRepository<TMSDataAccessLayer.TMSDB.Roles> _rolesRepository;
        public RoleService(TMSGenericRepository<TMSDataAccessLayer.TMSDB.Roles> rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        public async Task<IEnumerable<TMSDataAccessLayer.TMSDB.Roles>> GetAllRoles()
        {
            return await _rolesRepository.GetAll();
        }
    }
}
