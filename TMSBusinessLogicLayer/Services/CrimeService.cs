using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Repository;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public class CrimeService : ICrimeService
    {
        private readonly TMSGenericRepository<DriverLicense> _driverLicenseRepository;
        private readonly TMSGenericRepository<VehicleInsurance> _vehicleInsurance;
        private readonly TMSGenericRepository<TrafficMonitorTransactions> _trafficMonitorTransactions;
        private readonly TMSGenericRepository<Crimes> _crimeCharge;


        public CrimeService(TMSGenericRepository<DriverLicense> driverLicenseRepository, TMSGenericRepository<VehicleInsurance> vehicleInsurance,
                                        TMSGenericRepository<TrafficMonitorTransactions> trafficMonitorTransactions,
                                        TMSGenericRepository<Crimes> crimeCharge
                                        )
        {


            _driverLicenseRepository = driverLicenseRepository;
            _vehicleInsurance = vehicleInsurance;
            _trafficMonitorTransactions = trafficMonitorTransactions;
            _crimeCharge = crimeCharge;

        }


        public async Task<CrimeChargeOutputHandler> Charged(CrimeDTO crimeDTO)
        {
            var unsettledCrimes = 0;
            CrimeChargeOutputHandler outputHandler = new CrimeChargeOutputHandler();
            var licenseDetails = await _driverLicenseRepository.GetItemWithChildEntity(v => v.LicenseNumber == crimeDTO.LicenseNumber, "Person");

            if (licenseDetails == null)
            {

                bool isFinedAlready = await IsAlreadyFined(crimeDTO.LicenseNumber, "LNE");
                if (!isFinedAlready)
                {

                    var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("LNE"));

                    //Crime Committed 

                    var transaction = new TrafficMonitorTransactions
                    {
                        CrimeName = "LNE",
                        CreatedBy = crimeDTO.LoggedInUser,
                        DateCreated = DateTime.UtcNow.AddHours(2),
                        VehicleRegistrationNumber = crimeDTO.VehicleRegistrationNumber,
                        CrimeCharge = crime.Charge,
                        LicenseNumber = crimeDTO.LicenseNumber
                    };

                    await _trafficMonitorTransactions.AddAsync(transaction);
                    await _trafficMonitorTransactions.SaveChanges();

                    outputHandler.IsLicenseCrimeClean = false;
                    outputHandler.LicenseMessage = $"The license does exist, Driver does not have a license, The charge for this crime is {crime.Charge}";


                }
                else
                {

                    outputHandler.IsLicenseCrimeClean = false;
                    outputHandler.LicenseMessage = $"NON-Existant License: This Crime has already been Fine today for this driver";


                }
            }
            else
            {
                if (licenseDetails.ExpiryDate < DateTime.UtcNow.AddHours(2))
                {

                    bool isFinedAlready = await IsAlreadyFined(crimeDTO.LicenseNumber, "LDE");
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
                                CreatedBy = crimeDTO.LoggedInUser,
                                DateCreated = DateTime.UtcNow.AddHours(2),
                                VehicleRegistrationNumber = "N/A",
                                CrimeCharge = crime.Charge,
                                LicenseNumber = crimeDTO.LicenseNumber
                            };

                            await _trafficMonitorTransactions.AddAsync(transaction);
                            await _trafficMonitorTransactions.SaveChanges();


                            outputHandler.IsLicenseCrimeClean = false;
                            outputHandler.LicenseMessage = $"License expired on {licenseDetails.ExpiryDate}, which is over 30 days, Charge for the crime is {crime.Charge}";
                             
                        }

                        outputHandler.IsLicenseCrimeClean = false;
                        outputHandler.LicenseMessage = $"License expired on {licenseDetails.ExpiryDate}, it has to be renewed in 30 days";
                        

                    }
                    else
                    {

                        outputHandler.IsLicenseCrimeClean = false;
                        outputHandler.LicenseMessage = $"License Expired, This Crime has already been Fine today for this driver";



                    }



                }
                else
                {
                    var transactions = await _trafficMonitorTransactions.GetListAsync(c => c.LicenseNumber == crimeDTO.LicenseNumber && c.Paid.Equals(false));
                    if (transactions.Count() > 0)
                    {
                        unsettledCrimes = transactions.Count();
                    }
                }

            }

            //Registration 


            var vehicleInsurance = await _vehicleInsurance.GetItem(v => v.RegistrationNumber == crimeDTO.VehicleRegistrationNumber);

            if (vehicleInsurance == null)
            {
                bool isFinedAlready = await IsAlreadyFined(crimeDTO.VehicleRegistrationNumber, "NI");
                if (!isFinedAlready)
                {
                    var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("NI")); //not insured

                    //Crime Committed 

                    var transaction = new TrafficMonitorTransactions
                    {
                        CrimeName = "NI",
                        CreatedBy = crimeDTO.LoggedInUser,
                        DateCreated = DateTime.UtcNow.AddHours(2),
                        VehicleRegistrationNumber = crimeDTO.VehicleRegistrationNumber,
                        CrimeCharge = crime.Charge,
                        LicenseNumber = "N/A"
                    };

                    await _trafficMonitorTransactions.AddAsync(transaction);
                    await _trafficMonitorTransactions.SaveChanges();

                    outputHandler.IsRegistrationNumberCrimeClean = false;
                    outputHandler.RegNumberMessage = "This vehicle is not insured.";
                    //Identifier = crimeDTO.VehicleRegistrationNumber

                }
                else
                {

                    outputHandler.IsRegistrationNumberCrimeClean = false;
                    outputHandler.RegNumberMessage = "Vehicle not insured, Fine has already been charged today car/Driver.";
                        //Identifier = RegistrationNumber
                    
                }
            }
            else
            {
                if (!vehicleInsurance.IsActive || vehicleInsurance.ExpiryDate < DateTime.UtcNow.AddHours(2))
                {

                    bool isFinedAlready = await IsAlreadyFined(crimeDTO.VehicleRegistrationNumber, "NI");
                    if (!isFinedAlready)
                    {

                        var crime = await _crimeCharge.GetItem(x => x.CrimeName.Equals("IE")); //not insured

                        //check if already fined today
                        //Crime Committed 

                        var transaction = new TrafficMonitorTransactions
                        {
                            CrimeName = "IE",
                            CreatedBy = crimeDTO.LoggedInUser,
                            DateCreated = DateTime.UtcNow.AddHours(2),
                            VehicleRegistrationNumber = crimeDTO.VehicleRegistrationNumber,
                            CrimeCharge = crime.Charge,
                            LicenseNumber = "N/A"
                        };

                        await _trafficMonitorTransactions.AddAsync(transaction);
                        var output = await _trafficMonitorTransactions.SaveChanges();

                        outputHandler.IsRegistrationNumberCrimeClean = false;
                        outputHandler.RegNumberMessage = $"Insurance Expired, Charge for this crime is {crime.Charge}";
                          //  Identifier = crimeDTO.VehicleRegistrationNumber
                        
                    }
                    else
                    {

                        outputHandler.IsRegistrationNumberCrimeClean = false;
                        outputHandler.RegNumberMessage = $"Insurance Expired,This fine has already been charged today for this car/Driver.";
                          //  Identifier = RegistrationNumber
                         
                    }
                }
                var transactions = await _trafficMonitorTransactions.GetListAsync(c => c.VehicleRegistrationNumber == crimeDTO.VehicleRegistrationNumber && c.Paid.Equals(false));
                if (transactions.Count() > 0)
                {
                    unsettledCrimes = transactions.Count();
                }

            }


            //Charging the specific cars
            var crimedetails = await _crimeCharge.GetItem(x => x.Description.Equals(crimeDTO.CrimeName));

            CrimeDTO crimeReport = new CrimeDTO
            {
                CrimeName = crimeDTO.CrimeName,
                Description = crimeDTO.CrimeName,
                CrimeCharge = crimedetails.Charge,
                LicenseNumber = crimeDTO.LicenseNumber,
                VehicleRegistrationNumber = crimeDTO.VehicleRegistrationNumber,
                LoggedInUser = crimeDTO.LoggedInUser,
                DateCreated = DateTime.UtcNow.AddHours(2)


            };

            var mappedcrime = new AutoMapper<CrimeDTO, TrafficMonitorTransactions>().MapToObject(crimeReport);
            mappedcrime.Paid = false;
            mappedcrime.CreatedBy = crimeReport.LoggedInUser;
            mappedcrime.DateCreated = DateTime.UtcNow.AddHours(2);
            await _trafficMonitorTransactions.AddAsync(mappedcrime);
            await _trafficMonitorTransactions.SaveChanges();

            outputHandler.IsErrorOccured = false;
            outputHandler.Result = crimeReport;
            outputHandler.NumberOfCrimes = unsettledCrimes;

            return outputHandler;

        }

        public async Task<IEnumerable<Crimes>> GetCrimes()
        {
            return await _crimeCharge.GetAll();
        }

        public async Task<IEnumerable<TrafficMonitorTransactions>> GetAllTransaction(DateTime start, DateTime end, bool isDateRangeProvided)
        {
            DateTime startDate = Convert.ToDateTime(start.ToShortDateString());
            DateTime endDate = end.Date;
            if (isDateRangeProvided)
            {
                if (start.Date == end.Date)
                {
                    return await _trafficMonitorTransactions.GetListAsync(x => x.DateCreated.Date == start.Date);

                }
                return await _trafficMonitorTransactions.GetListAsync(x => x.DateCreated.Date >= start.Date && x.DateCreated <= end.Date);
            }
            else
            {

                return await _trafficMonitorTransactions.GetAll();

            }
        }

        public Task<IEnumerable<TrafficMonitorTransactions>> GetRegistrationCharges(string regnumber,string checkType)
        {
            if (checkType == null)
            {
                checkType = "V";
            }
            if (checkType.Equals("L"))
            {
                return _trafficMonitorTransactions.GetListAsync(x => x.LicenseNumber == regnumber);

            }
            else
            {
                return _trafficMonitorTransactions.GetListAsync(x => x.VehicleRegistrationNumber == regnumber);
            }
        }

        public async Task<bool> IsAlreadyFined(string item, string identifier)
        {
            var crimes = await _trafficMonitorTransactions.GetAll();
            int count = 0;
            foreach (var crime in crimes)
            {
                if (crime.CrimeName == identifier && crime.DateCreated.ToShortDateString() == DateTime.UtcNow.AddHours(2).ToShortDateString())
                {
                    count = count + 1;
                }

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

        

    }
}
