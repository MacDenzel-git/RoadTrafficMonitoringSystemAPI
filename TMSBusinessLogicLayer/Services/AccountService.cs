using TMSBusinessLogicLayer.Services;

 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
 
using TMSBusinessLogicLayer.Services;
using TMSDataAccessLayer.TMSDB;
using TMSBusinessLogicLayer.Repository;
using TMSDataAccessLayer;

namespace TMSBusinessLogicLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly TMSGenericRepository<Credentials> _credentialRepository;
        private readonly TMSGenericRepository<PersonalDetails> _personDetails;
        private readonly TMSGenericRepository<Positions> _position;
        private readonly TMSGenericRepository<VehicleInsurance> _vehicles;
        private readonly TMSGenericRepository<TrafficMonitorTransactions> _crimes;
        private readonly TMSGenericRepository<DriverLicense> _licenses;

        private IPasswordRecover _passwordRecover;

        public AccountService(TMSGenericRepository<DriverLicense> licenses,TMSGenericRepository<VehicleInsurance> vehicles ,TMSGenericRepository<TrafficMonitorTransactions> crimes, IPasswordRecover passwordRecover, TMSGenericRepository<Credentials> credentialRepository, TMSGenericRepository<PersonalDetails> personDetails, TMSGenericRepository<Positions> position)
        {
            _licenses = licenses;
            _vehicles = vehicles;
            _crimes = crimes;
            _passwordRecover = passwordRecover;
            _credentialRepository = credentialRepository;
            _personDetails = personDetails;
            _position = position;
        }

        public async Task<OutputHandler> Login(LoginDTO loginRequest)
        {

            if (loginRequest.Password == null)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "Password cannot be empty"
                };

            }
            if (loginRequest.Username == null)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "Password"
                };
            }

            var authorisedCredentials = await _credentialRepository.GetItem(x => x.Password.Equals(loginRequest.Password) && x.Username.Equals(loginRequest.Username));
            if (authorisedCredentials != null)
            {
                if (authorisedCredentials.IsActive == false)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = "This Account is available but current Not Active, Please contact Administrator to Activate your Account"
                    };
                }
                //This code gets the record associated with the credentials 
                //the username is set when registering and it the email set by the user 
                var position = await _personDetails.GetItem(x => x.EmailAddress.Equals(loginRequest.Username));

                if (position == null)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = "Something went wrong, please try again"
                    };
                }
                else
                {
                    var MappedPersonalDetails = new AutoMapper<PersonalDetails, PersonalDetailsDTO>().MapToObject(position);

                    return new OutputHandler
                    {
                        IsErrorOccured = false,
                        Result = MappedPersonalDetails
                    };
                }

            }
            else
            {
                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "Account not found, Check your password or username and try again."
                };
            }
        }

        public async Task<OutputHandler> CreateCredentials(CredentialDTO credentials)
        {
            var mappedCredetials = new AutoMapper<CredentialDTO, Credentials>().MapToObject(credentials);
            credentials.IsActive = false;
            await _credentialRepository.AddAsync(mappedCredetials);
            var output = await _credentialRepository.SaveChanges();
            if (output.IsErrorOccured)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "Something went wrong please try again"

                };
            }
            else
            {
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Registered Succesfully"

                };
            }

        }

        public async Task<OutputHandler> CreateUser(UserDTO user)
        {
            try
            {
                bool isExist = await _personDetails.AnyAsync(x => x.EmailAddress == user.EmailAddress);
                if (isExist)
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = "This email already exist in the system",
                        IsErrorKnown = true

                    };
                }

                var mappedUser = new AutoMapper<UserDTO, PersonalDetails>().MapToObject(user);
                if (user.Password == null)
                {
                    user.Password = "Default";
                }
                await _personDetails.AddAsync(mappedUser);

                var output = await _personDetails.SaveChanges();
                if (output.IsErrorOccured)
                {

                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = "Something went wrong please try again"

                    };
                }
                else
                {
                    // create credentialls 
                    var newCredentials = new CredentialDTO
                    {
                        Username = mappedUser.EmailAddress,
                        Password = user.Password,
                        PersonalDetailsId = mappedUser.PersonalDetailsId

                    };
                    var result = await CreateCredentials(newCredentials);
                    await _passwordRecover.EmailMember("denzelmac8@gmail.com", "", "Activation");
                    await _passwordRecover.EmailMember("denzelmac8@gmail.com", "", "Registration");
                   
                    return new OutputHandler
                    {
                        IsErrorOccured = false,
                        Message = "Registered Succesfully",
                        Result = newCredentials

                    };
                }

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<OutputHandler> ChangeUserCredentialStatus(long personaldetailId)
        {
           
            var output = await _credentialRepository.GetItem(x => x.PersonalDetailsId == personaldetailId);
            if (output.IsActive)
            {
                output.IsActive = false;
                var result = _credentialRepository.UpdateAsync(output);

                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Deactivated"
                    
                };
            }
            else
            {
                             
                output.IsActive = true;
                var result = _credentialRepository.UpdateAsync(output);
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Activated"
                    
                    
                };
            }
        }

        public async Task<IEnumerable<UserDetailDTO>> GetAllUsers()
        {
            try
            {
                var users = await
                   _personDetails.GetAllWithChildEntity("Credentials");


                var mapped = new AutoMapper<PersonalDetails, UserDetailDTO>().MapToList(users).ToList();

                foreach (var item in mapped)
                {
                    var position = await _position.GetItem(x => x.PositionId == item.PositionId);
                    var credentials = await _credentialRepository.GetItem(x => x.PersonalDetailsId == item.PersonalDetailsId);

                    item.PositionName = position.PositionName;
                    item.IsActive = credentials.IsActive;
                   
                }
                //foreach (var item in users.FirstOrDefault().Credentials)
                //{
                //    item.IsActive = mapped.FirstOrDefault().IsActive;
                //}
                return mapped;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<OutputHandler> ChangeUserRole(long personaldetailId)
        {

            var output = await _personDetails.GetItem(x => x.PersonalDetailsId == personaldetailId);
            if (output.RoleId == 1)
            {
                output.RoleId = 2;
                var result = _personDetails.UpdateAsync(output);

                return new OutputHandler
                {
                    IsErrorOccured = false,

                };
            }
            else
            {

                output.RoleId = 1;
                var result = _personDetails.UpdateAsync(output);
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Activated"


                };
            }
        }

        public async Task<Totals> Count()
        {
            var vehicles = await _vehicles.GetAll();
            var crimes = await _crimes.GetAll();
            var licensesList = await _licenses.GetAll();
            var crimesToday = await _crimes.GetListAsync(x => x.DateCreated.Day == DateTime.Now.Day && x.DateCreated.Year == DateTime.Now.Year &&  x.DateCreated.Month == DateTime.Now.Month);
            Totals totals = new Totals
            {
                VehiclesRegistered = vehicles.Count(),
                CrimesCount = crimes.Count(),
                Licenses = licensesList.Count(),
                CrimesToday = crimesToday.Count()
            };

            return totals;
        }

        public async Task<OutputHandler> UpdateUser(UserDTO user)
        {
            try
            {

                var mappedUser = new AutoMapper<UserDTO, PersonalDetails>().MapToObject(user);
                //mappedUser.ModifiedDate = DateTime.UtcNow.AddHours(2);
                //mappedUser.ModifiedBy = user.EmailAddress;
               
                await _personDetails.UpdateAsync(mappedUser);
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Updated Succesfully"
                };


            }
            catch (Exception e)
            {

                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "Something went wrong, Please contact the administrator"

                };
            }
        }

        public async Task<UserDetailDTO> GetUserById(long personId)
        {
            var user = await _personDetails.GetItem(x => x.PersonalDetailsId == personId);
            var mappedUser = new AutoMapper<PersonalDetails, UserDetailDTO>().MapToObject(user);

            return mappedUser;
        }

        public async Task<OutputHandler> DeleteUserById(long personId)
        {
            try
            {
                var personToDelete = await _personDetails.GetItem(x => x.PersonalDetailsId == personId);
                var credentialsToDelete = await _credentialRepository.GetItem(x => x.PersonalDetailsId == personId);
                var licenseDelete = await _licenses.GetItem(x => x.PersonId == personId);
                if (licenseDelete != null)
                {
                    _licenses.DeleteAsync(licenseDelete);
                    await _licenses.SaveChanges();
                }
                _credentialRepository.DeleteAsync(credentialsToDelete);
                await _credentialRepository.SaveChanges();
                _personDetails.DeleteAsync(personToDelete);
                await _personDetails.SaveChanges();
                return new OutputHandler { IsErrorOccured = false, Message = "User Deleted Successfully" };
            }
            catch (Exception ex)
            {

                return new OutputHandler { IsErrorOccured = true, Message = "Deleted Failed" };

            }
        }
    }
}