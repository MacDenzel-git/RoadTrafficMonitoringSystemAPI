using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface IVehicleInsuranceService
    {
        Task<OutputHandler> CreateVehicle(VehicleInsurance vehicle);
        Task<VehicleInsurance> GetVehicle(long id);
        Task<OutputHandler> UpdateVehicle(VehicleInsurance vehicle);
        Task<IEnumerable<VehicleInsurance>> GetAllVehicles();
        Task DeleteVehicle(VehicleInsurance vehicle);
    }
}
