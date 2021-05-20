using System.Collections.Generic;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface IDriverLicenseService
    {
        Task<OutputHandler> CreateDriversLicense(DriverLicenseDTO driverLicenseDTO);
        Task<OutputHandler> DeleteDriversLicense(string driverLicense);
        Task<IEnumerable<DriverLicenseDTO>> GetAllDriversLicense();
        Task<DriverLicenseDTO> GetDriversLicense(string licenseNumber);
        Task<DriverLicenseDTO> GetDriversLicenseById(int personId);
        Task<OutputHandler> UpdateDriversLicense(DriverLicenseDTO driverLicenseDTO);
    }
}