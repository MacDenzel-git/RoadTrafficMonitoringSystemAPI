using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Repository;
using TMSBusinessLogicLayer.Services.PasswordRecover;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public class DriverLicenseService : IDriverLicenseService
    {
        private readonly TMSGenericRepository<DriverLicense> _repository;
        private readonly TMSGenericRepository<PersonalDetails> _person;
        private readonly TMSDBContext _context;
        public DriverLicenseService(TMSGenericRepository<PersonalDetails> person,TMSGenericRepository<DriverLicense> repository, TMSDBContext context)
        {
            _context = context;
            _repository = repository;
            _person = person;
        }

        public async Task<OutputHandler> CreateDriversLicense(DriverLicenseDTO driverLicenseDTO)
        {
            try
            {
                var isExist = await _repository.GetItem(x => x.PersonId == driverLicenseDTO.PersonId);
                if (isExist != null)
                {
                    if (isExist.ExpiryDate < DateTime.UtcNow.AddHours(2))
                    {
                        var updateLicense = new AutoMapper<DriverLicenseDTO, DriverLicense>().MapToObject(driverLicenseDTO);
                        isExist.ExpiryDate = isExist.DateIssued.AddYears(5);
                        await _repository.UpdateAsync(updateLicense);
                        
                    }
                    return new OutputHandler { IsErrorOccured = false, Message = "Already Exist" };
                }
                var driverLicense = new AutoMapper<DriverLicenseDTO, DriverLicense>().MapToObject(driverLicenseDTO);
                var person = await _person.GetItem(x => x.PersonalDetailsId == driverLicenseDTO.PersonId);
                var year = driverLicenseDTO.DateIssued.Year;
                var month = driverLicenseDTO.DateIssued.Month;
                var day = driverLicenseDTO.DateIssued.Day;
                var seconds = driverLicense.DateIssued.Second;
                var firstname = person.FirstName.Substring(0, 1);
                var lastname = person.LastName.Substring(0, 3);

                driverLicense.LicenseNumber = $"{firstname}{lastname}{year}{month}{day}{seconds}{person.PositionId}";
                driverLicense.Trn = $"Trn{firstname}{year}{month}{day}{seconds}{person.PositionId}";


                await _repository.CreateAsync(driverLicense);
                await _repository.SaveChanges();
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Drivers License created successfully"
                };
            }
            catch (Exception ex)
            {
                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = ex.Message
                };
            }
        }
        public async Task<DriverLicenseDTO> GetDriversLicense(string licenseNumber)
        {
            var driversLicense = await _repository.GetItem(x => x.LicenseNumber == licenseNumber);
            var driverLicense = new AutoMapper<DriverLicense, DriverLicenseDTO>().MapToObject(driversLicense);
            return driverLicense;
        }

        public async Task<DriverLicenseDTO> GetDriversLicenseById(int personId)
        {
            var driversLicense = await _repository.GetItem(x => x.PersonId == personId);
            var driverLicense = new AutoMapper<DriverLicense, DriverLicenseDTO>().MapToObject(driversLicense);
            return driverLicense;
        }
        public async Task<OutputHandler> UpdateDriversLicense(DriverLicenseDTO driverLicenseDTO)
        {
            try
            {
                var driverLicense = await _repository.GetItem(x => x.LicenseNumber == driverLicenseDTO.LicenseNumber);

                driverLicense.LicenseNumber = driverLicenseDTO.LicenseNumber;
                                driverLicense.Trn = driverLicenseDTO.Trn;
                driverLicense.FirstIssue = driverLicenseDTO.FirstIssue;
                driverLicense.DateIssued = driverLicenseDTO.DateIssued;
                driverLicense.ExpiryDate = driverLicenseDTO.ExpiryDate;
                driverLicense.CountryIssued = driverLicenseDTO.CountryIssued;
                driverLicense.LicenseCode = driverLicenseDTO.LicenseCode;
                driverLicense.VehicleRestriction = driverLicenseDTO.VehicleRestriction;
                driverLicense.DriverRestriction = driverLicenseDTO.DriverRestriction;
                driverLicense.IssueNumber = driverLicenseDTO.IssueNumber;
                driverLicense.PersonId = driverLicenseDTO.PersonId;

                await _repository.SaveChanges();
                return new OutputHandler
                {
                    IsErrorOccured = false,
                    Message = "Drivers license details updated successfully"
                };
            }
            catch (Exception ex)
            {

                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = ex.Message
                };
            }
        }
        public async Task<IEnumerable<DriverLicenseDTO>> GetAllDriversLicense()
        {
            var driverLicenses = await _repository.GetAll();
            var driverLicensesMapping = new AutoMapper<DriverLicense, DriverLicenseDTO>().MapToList(driverLicenses);
            return driverLicensesMapping;
        }
        public async Task<OutputHandler> DeleteDriversLicense(string driverLicense)
        {
            var outputHandler = await _repository.DeleteByIdAsync(driverLicense);
            await _repository.SaveChanges();
            return outputHandler;
        }
    }
}
