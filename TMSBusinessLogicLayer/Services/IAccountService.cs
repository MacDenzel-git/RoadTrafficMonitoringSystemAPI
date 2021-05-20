 
using System.Collections.Generic;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface IAccountService
    {
        Task<OutputHandler> CreateCredentials(CredentialDTO credentials);
        Task<OutputHandler> Login(LoginDTO loginRequest);
        Task<OutputHandler> CreateUser(UserDTO user);
        Task<OutputHandler> ChangeUserCredentialStatus(long PersonalDetailsId);//Activate or DeactivateUSer 
        Task<IEnumerable<UserDetailDTO>> GetAllUsers();
        Task<OutputHandler> ChangeUserRole(long personalDetailId);
        Task<Totals> Count();
        Task<UserDetailDTO> GetUserById(long personId);
        Task<OutputHandler> DeleteUserById(long personId);
        Task<OutputHandler> UpdateUser(UserDTO user);

    }
}
