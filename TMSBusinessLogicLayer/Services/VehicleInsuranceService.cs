using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.Repository;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public class VehicleInsuranceService : IVehicleInsuranceService
    {
        private readonly TMSGenericRepository<VehicleInsurance> _vehicleInsuranceRepository;
        public VehicleInsuranceService(TMSGenericRepository<VehicleInsurance> vehicleInsuranceRepository)
        {
            _vehicleInsuranceRepository = vehicleInsuranceRepository;
        }
        public async Task<OutputHandler> CreateVehicle(VehicleInsurance vehicle)
        {
            try
            {
                await _vehicleInsuranceRepository.AddAsync(vehicle);
                await _vehicleInsuranceRepository.SaveChanges();
                return new OutputHandler { IsErrorOccured = false, Message = "Vehicle Insurance Detail Added Successfully" };

            }
            catch (Exception)
            {

                return new OutputHandler {IsErrorOccured = true,Message="Something went wrong, please report the problem to the administrator"  };
            }     
        }

        public Task DeleteVehicle(VehicleInsurance vehicle)
        {
             _vehicleInsuranceRepository.DeleteAsync(vehicle);
           return _vehicleInsuranceRepository.SaveChanges();
        }

        public async Task<IEnumerable<VehicleInsurance>> GetAllVehicles()
        {
          
                return await _vehicleInsuranceRepository.GetAll( );
  
 
        }

        public async Task<VehicleInsurance> GetVehicle(long id)
        {
            
                return await _vehicleInsuranceRepository.GetItem(x => x.VehicleId == id);
        }

        public async  Task<OutputHandler> UpdateVehicle(VehicleInsurance  vehicle)
        {
            try
            {
                var updatedVehicle = new VehicleInsurance();
                updatedVehicle = vehicle;
                await _vehicleInsuranceRepository.UpdateAsync(updatedVehicle);
                return new OutputHandler { IsErrorOccured = false, Message = "Vehicle Insurance Detail Added Successfully" };

            }
            catch (Exception ex)
            {

                return new OutputHandler { IsErrorOccured = true, Message = $"{ex.Message}Something went wrong, please report the problem to the administrator" };
            }
        }
    }
}
