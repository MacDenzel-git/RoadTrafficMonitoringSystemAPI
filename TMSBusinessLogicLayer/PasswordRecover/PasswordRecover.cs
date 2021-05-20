using TMSWebAPI.Controllers;
 
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Repository;
using TMSBusinessLogicLayer.Services;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services.PasswordRecover
{
    public class PasswordRecover : IPasswordRecover
    {
        public readonly TMSGenericRepository<RecoveryData> _recoveryDataRepository;
        public readonly TMSGenericRepository<Credentials> _credentialsRepository;
        public readonly TMSGenericRepository<PersonalDetails> _personDetailsRepository;



        public PasswordRecover(TMSGenericRepository<PersonalDetails> personDetailsRepository, TMSGenericRepository<RecoveryData> recoveryDataRepository, TMSGenericRepository<Credentials> credentialsRepository)
        {
            _recoveryDataRepository = recoveryDataRepository;
            _credentialsRepository = credentialsRepository;
            _personDetailsRepository = personDetailsRepository;
        }
        public async Task AddRecoveryInformation(string email, string otp)
        {
            RecoveryData recoveryData = new RecoveryData
            {
                Email = email,
                Otp = otp,
                CreatedDate = DateTime.UtcNow.AddHours(2)
            };

            await _recoveryDataRepository.AddAsync(recoveryData);
            await _recoveryDataRepository.SaveChanges();
        }

        public async Task<RecoveryData> GetRecoveryInformation(string email)
        {
            return await _recoveryDataRepository.GetItem(x => x.Email == email);
        }

        public async Task<OutputHandler> EmailMember(string email, string otp, string action = null)
        {
            try
            {
                var body = "";
                if (action.Equals("Recover"))
                {
                    body = string.Format("Your One time Password is {0}, it expires in 20 minutes. Thank you", otp);
                }
                else if (action.Equals("Activation"))
                {
                    var newAccount = await _personDetailsRepository.GetItemWithChildEntity(X => X.EmailAddress.Equals(email), "Position");
                    body = $"An account has been registered with the following Details <hr /> <b>Position:</b> {newAccount.Position} <br /> <b>Name:</b> {newAccount.FirstName} {newAccount.LastName} <br /> <b>Email:</b> {newAccount.EmailAddress} <br /> <b>Phone Number:</b> {newAccount.PhoneNumber} <br /><br /> <h3 style=\"color:red\"> Please verify this information and Activate Account<br />";
                }
                else
                {
                    var newAccount = await _personDetailsRepository.GetItemWithChildEntity(X => X.EmailAddress.Equals(email), "Position");
                    body = string.Format("Thank you for registering, below are your details <hr /> <b>Position:</b> {1} <br /> <b>Name:</b> {5} {0} <br /> <b>Date of Birth:</b> {5} <br /> <b>Email:</b> {2} <br /> <b>Phone Number:</b> {3} <br /><br /> <h3 style= {4}color:red{4}> Please verify this information your account will be activated within 2 hours<br />", newAccount.FirstName, newAccount.Position.PositionName, newAccount.EmailAddress, newAccount.PhoneNumber, "\"", newAccount.DateOfBirth,newAccount.LastName);

                }

                //Move configuration to WebConfig
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.From = new MailAddress("denzelmac8@gmail.com");  // replace with valid value
                if (action.Equals("Recover"))
                {
                    message.Subject = "TMS PASSWORD RECOVERY";
                }
                else if (action.Equals("Activation"))
                {
                    message.Subject = "TMS REGISTRATION";
                }
                else
                {
                    message.Subject = "TMS REGISTRATION";
                }

                message.Body = string.Format(body);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "denzelmac8@gmail.com",  // replace with valid value
                        Password = "joxntoczjyytjdsh"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return new OutputHandler { IsErrorOccured = false };
                }
            }
            catch (Exception e)
            {
                return new OutputHandler { IsErrorOccured = true, Message = "Something went wrong, Please Ensure that your email is valid and try gain" };
            }
        }

        public async Task<OutputHandler> DeleteOTPRecord(string username)
        {
            var credentials = await _credentialsRepository.GetItem(x => x.Username == username);
            var result = await _recoveryDataRepository.GetItem(x => x.Email.Equals(credentials.Username));
            try
            {
                _recoveryDataRepository.DeleteAsync(result);
                await _recoveryDataRepository.SaveChanges();
                return new OutputHandler { IsErrorOccured = false };
            }
            catch (Exception ex)
            {
                return new OutputHandler
                { IsErrorOccured = true, Message = ex.Message };
            }
        }

        public async Task<OutputHandler> UpdateCredentials(LoginViewModel loginDetails)
        {
            try
            {
                var oldCredentials = await _credentialsRepository.GetItem(x => x.Username == loginDetails.Username);
                oldCredentials.Password = loginDetails.Password;
                await _credentialsRepository.UpdateAsync(oldCredentials);
                return new OutputHandler
                {
                    IsErrorOccured = false
                };
            }
            catch (Exception)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true
                };
            }



        }
        public async Task<Credentials> GetCredentials(string username)
        {
            return await _credentialsRepository.GetItem(x => x.Username == username);

        }

        public string RandomPassword()
        {
            PasswordGenerator passwordGenerator = new PasswordGenerator();
            StringBuilder builder = new StringBuilder();
            builder.Append(passwordGenerator.RandomString(4, true));
            builder.Append(passwordGenerator.RandomNumber(1000, 9999));
            builder.Append(passwordGenerator.RandomString(2, false));
            return builder.ToString();
        }
    }
}
