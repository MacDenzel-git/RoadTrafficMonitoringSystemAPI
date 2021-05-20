using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TMSBusinessLogicLayer.Services.Roles
{
    public interface IRoleService
    {
        Task<IEnumerable<TMSDataAccessLayer.TMSDB.Roles>> GetAllRoles();

    }
}
