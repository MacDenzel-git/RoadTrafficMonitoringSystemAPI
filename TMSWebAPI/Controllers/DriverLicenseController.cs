using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Repository;
using TMSBusinessLogicLayer.Services;
using TMSDataAccessLayer;

using TMSDataAccessLayer.TMSDB;

namespace TMSWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/DriverLicense")]
    [ApiController]
    public class DriverLicenseController : ControllerBase
    {
        private readonly TMSGenericRepository<DriverLicense> _driverLicenseRepository;
        private readonly TMSGenericRepository<VehicleInsurance> _vehicleInsurance;
        private readonly TMSGenericRepository<TrafficMonitorTransactions> _trafficMonitorTransactions;
        private readonly TMSGenericRepository<Crimes> _crimeCharge;
        private readonly IDriverLicenseService _driverLicenseService;


        public DriverLicenseController(TMSGenericRepository<DriverLicense> driverLicenseRepository,
                                        TMSGenericRepository<VehicleInsurance> vehicleInsurance,
                                        TMSGenericRepository<TrafficMonitorTransactions> trafficMonitorTransactions,
                                        TMSGenericRepository<Crimes> crimeCharge,
                                        IDriverLicenseService driverLicenseService
                                        )
        {


            _driverLicenseRepository = driverLicenseRepository;
            _vehicleInsurance = vehicleInsurance;
            _trafficMonitorTransactions = trafficMonitorTransactions;
            _crimeCharge = crimeCharge;
            _driverLicenseService = driverLicenseService;
        }

        [HttpGet]
        public async Task<OutputHandler> Get(string loggedInUser, [FromQuery]string licenseNumber, [FromQuery] string vehicle)
        {

            var unsettledCrimes = 0;
            var licenseDetails = await _driverLicenseRepository.GetItemWithChildEntity(v => v.LicenseNumber == licenseNumber, "Person");

            if (licenseDetails == null)
            {

                bool isFinedAlready = await IsAlreadyFined(licenseNumber, "LNE","L");
                if (!isFinedAlready)
                {

                    var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("LNE"));

                    //Crime Committed 

                    var transaction = new TrafficMonitorTransactions
                    {
                        CrimeName = "LNE",
                        CreatedBy = loggedInUser,
                        DateCreated = DateTime.UtcNow.AddHours(2),
                        VehicleRegistrationNumber = vehicle,
                        CrimeCharge = crime.Charge,
                        LicenseNumber = licenseNumber
                    };

                    await _trafficMonitorTransactions.AddAsync(transaction);
                    await _trafficMonitorTransactions.SaveChanges();

                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = $"The license does exist, Driver does not have a license, The charge for this crime is {crime.Charge}"
                    };
                }
                else
                {
                    return new OutputHandler
                    {
                        IsErrorOccured = true,
                        Message = $"NON-Existant License: This Crime has already been Fine today for this driver"
                    };
                }
            }
            else
            {
                if (licenseDetails.ExpiryDate < DateTime.UtcNow.AddHours(2))
                {

                    bool isFinedAlready = await IsAlreadyFined(licenseNumber, "LDE");
                    if (!isFinedAlready)
                    {
                        var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("LED"));

                        var today = DateTime.UtcNow.AddHours(2);
                        var expiryDate = licenseDetails.ExpiryDate;

                        var result = (today - expiryDate).Days;
                        //Crime Committed 
                        if ((DateTime.UtcNow.AddHours(2) - licenseDetails.ExpiryDate).Days > 30)
                        {

                            var transaction = new TrafficMonitorTransactions
                            {
                                CrimeName = "LED",
                                CreatedBy = loggedInUser,
                                DateCreated = DateTime.UtcNow.AddHours(2),
                                VehicleRegistrationNumber = "N/A",
                                CrimeCharge = crime.Charge,
                                LicenseNumber = licenseNumber
                            };

                            await _trafficMonitorTransactions.AddAsync(transaction);
                            await _trafficMonitorTransactions.SaveChanges();

                            return new OutputHandler
                            {
                                IsErrorOccured = true,
                                Message = $"License expired on {licenseDetails.ExpiryDate}, which is over 30 days, Charge for the crime is {crime.Charge}",
                            };
                        }
                        return new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = $"License expired on {licenseDetails.ExpiryDate}, it has to be renewed in 30 days",
                        };

                    }
                    else
                    {

                        return new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = $"License Expired, This Crime has already been Fine today for this driver"
                        };

                    }



                }
                else
                {
                    var transactions = await _trafficMonitorTransactions.GetListAsync(c => c.LicenseNumber == licenseNumber && c.Paid.Equals(false));
                    if (transactions.Count() > 0)
                    {
                        unsettledCrimes = transactions.Count();
                    }
                }

            }

            LicenseViewModel licenseViewModel = new LicenseViewModel
            {
                Trn = licenseDetails.Trn,
                Firstname = licenseDetails.Person.FirstName,
                Lastname = licenseDetails.Person.LastName,
                PhoneNumber = licenseDetails.Person.PhoneNumber,
                FirstIssue = licenseDetails.FirstIssue,
                LicenseCode = licenseDetails.LicenseCode,
                LicenseNumber = licenseDetails.LicenseNumber,
                VehicleRestriction = licenseDetails.VehicleRestriction,
                ExpiryDate = licenseDetails.ExpiryDate

            };

            return new OutputHandler
            {
                IsErrorOccured = false,
                Result = licenseViewModel,
                NumberOfCrimes = unsettledCrimes

            };
        }

        [HttpGet("GetInsuranceDetails")]
        public async Task<OutputHandler> GetVehicleInsurances(string loggedInUser, string RegistrationNumber)

        {
            try
            {
                var unsettledCrimes = 0;

                var vehicleInsurance = await _vehicleInsurance.GetItem(v => v.RegistrationNumber == RegistrationNumber);

                if (vehicleInsurance == null)
                {
                    bool isFinedAlready = await IsAlreadyFined(RegistrationNumber, "NI");
                    if (isFinedAlready == false)
                    {
                        var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("NI") && x.IsActive); //not insured

                        //Crime Committed 

                        var transaction = new TrafficMonitorTransactions
                        {
                            CrimeName = "NI",
                            CreatedBy = loggedInUser,
                            DateCreated = DateTime.UtcNow.AddHours(2),
                            VehicleRegistrationNumber = RegistrationNumber,
                            CrimeCharge = crime.Charge,
                            LicenseNumber = "N/A"
                        };

                        await _trafficMonitorTransactions.AddAsync(transaction);
                        await _trafficMonitorTransactions.SaveChanges();

                        return new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = "This vehicle is not insured.",
                            Identifier = RegistrationNumber
                        };
                    }
                    else
                    {
                        return new OutputHandler
                        {
                            IsErrorOccured = true,
                            Message = "Vehicle not insured, Fine has already been charged today car/Driver.",
                            Identifier = RegistrationNumber
                        };
                    }
                }
                else
                {
                    if (!vehicleInsurance.IsActive || vehicleInsurance.ExpiryDate < DateTime.UtcNow.AddHours(2))
                    {

                        bool isFinedAlready = await IsAlreadyFined(RegistrationNumber, "NI");
                        if (isFinedAlready == false)
                        {

                            var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("IE")); //not insured

                            //check if already fined today
                            //Crime Committed 

                            var transaction = new TrafficMonitorTransactions
                            {
                                CrimeName = "IE",
                                CreatedBy = loggedInUser,
                                DateCreated = DateTime.UtcNow.AddHours(2),
                                VehicleRegistrationNumber = RegistrationNumber,
                                CrimeCharge = crime.Charge,
                                LicenseNumber = "N/A"
                            };

                            await _trafficMonitorTransactions.AddAsync(transaction);
                            var output = await _trafficMonitorTransactions.SaveChanges();

                            return new OutputHandler
                            {
                                IsErrorOccured = true,
                                Message = $"Insurance Expired, Charge for this crime is {crime.Charge}",
                                Identifier = RegistrationNumber
                            };
                        }
                        else
                        {
                            return new OutputHandler
                            {
                                IsErrorOccured = true,
                                Message = $"Insurance Expired,This fine has already been charged today for this car/Driver.",
                                Identifier = RegistrationNumber
                            };
                        }
                    }
                    var transactions = await _trafficMonitorTransactions.GetListAsync(c => c.VehicleRegistrationNumber == RegistrationNumber && c.Paid.Equals(false));
                    if (transactions.Count() > 0)
                    {
                        unsettledCrimes = transactions.Count();
                    }
                    else
                    {
                        return new OutputHandler
                        {
                            Result = vehicleInsurance
                        };
                    }
                }
                return new OutputHandler
                {
                    Result = vehicleInsurance
                };
            }
            catch (Exception e)
            {

                return new OutputHandler
                {
                    IsErrorOccured = true,
                    Message = "Something went wrong, please Logout and Try again"
                };
            }

        }

        [HttpGet("GetLicenseCharges")]
        public async Task<IEnumerable<TrafficMonitorTransactions>> GetLicenseCharges(string licenseNumber)
        {
            return await _trafficMonitorTransactions.GetListAsync(v => v.LicenseNumber == licenseNumber);
        }

        [HttpGet("GetRegistrationCharges")]
        public async Task<IEnumerable<TrafficMonitorTransactions>> GetRegistrationCharges(string registrationNumber)
        {

            return await _trafficMonitorTransactions.GetListAsync(v => v.VehicleRegistrationNumber == registrationNumber && v.Paid.Equals(false));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item">Car/License</param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        /// 
        [HttpGet("IsAlreadyFined")]
        public async Task<bool> IsAlreadyFined(string item, string identifier,string checkType = null)
        {
            TrafficMonitorTransactions crime = new TrafficMonitorTransactions();
            if (checkType == null)
            {
                 crime = await _trafficMonitorTransactions.GetItem(x => x.VehicleRegistrationNumber == item && x.CrimeName == identifier
                   && x.DateCreated.ToShortDateString() == DateTime.UtcNow.AddHours(2).ToShortDateString()); 
            }
            else
            {
                crime = await _trafficMonitorTransactions.GetItem(x => x.LicenseNumber == item && x.CrimeName == identifier
                  && x.DateCreated.ToShortDateString() == DateTime.UtcNow.AddHours(2).ToShortDateString());

            }
            int count = 0;
                if (crime != null)
                {
                    count = count + 1;
                }
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [HttpGet("GetDriversLicense")]
        public async Task<IActionResult> GetDriverLicenseAsync(string licenseNumber)
        {
            var driverLicense = await _driverLicenseService.GetDriversLicense(licenseNumber);
            return Ok(driverLicense);
        }

        [HttpGet("GetDriversLicenseById")]
        public async Task<IActionResult> GetDriverLicenseByIdAsync(int personId)
        {
            var driverLicense = await _driverLicenseService.GetDriversLicenseById(personId);
            return Ok(driverLicense);
        }

        [HttpGet("GetAllDriversLicense")]
        public async Task<IActionResult> GetAllDriverLicenseAsync()
        {
            var driverLicenses = await _driverLicenseService.GetAllDriversLicense();
            return Ok(driverLicenses);
        }

        [HttpDelete("DeleteDriversLicense")]
        public async Task<IActionResult> DeleteAllDriverLicenseAsync(string driverLicense)
        {
            var outputHandler = await _driverLicenseService.DeleteDriversLicense(driverLicense);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        [HttpPut("UpdateDriversLicense")]
        public async Task<IActionResult> UpdateDriversLicense(DriverLicenseDTO driverLicenseDTO)
        {
            var outputHandler = await _driverLicenseService.UpdateDriversLicense(driverLicenseDTO);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }

        [HttpPost("CreateDriversLicense")]
        public async Task<IActionResult> CreateDriversLicense(DriverLicenseDTO driverLicenseDTO)
        {
            driverLicenseDTO.CountryIssued = "Malawi"; //it's a malawian system so issueing malawi licenses

            var outputHandler = await _driverLicenseService.CreateDriversLicense(driverLicenseDTO);
            if (outputHandler.IsErrorOccured)
            {
                return BadRequest(outputHandler);
            }
            return Ok(outputHandler);
        }
    }

}