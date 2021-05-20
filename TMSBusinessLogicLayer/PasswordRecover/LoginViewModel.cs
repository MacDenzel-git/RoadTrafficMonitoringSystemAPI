 
using TMSBusinessLogicLayer.Services;

namespace TMSWebAPI.Controllers
{
    public class LoginViewModel : OutputHandler
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } //used for change password

    }
}