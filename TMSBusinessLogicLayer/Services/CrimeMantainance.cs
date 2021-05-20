using System;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSBusinessLogicLayer.Repository;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public class CrimeMantainance : ICrimeMantainance
    {
        private readonly TMSGenericRepository<Crimes> _crimes;
        public CrimeMantainance(TMSGenericRepository<Crimes> crimes)
        {
            _crimes = crimes;
        }
        public async Task<OutputHandler> Create(CrimeMantainanceDTO crime)
        {
            try
            {
                var mapped = new AutoMapper<CrimeMantainanceDTO, Crimes>().MapToObject(crime);
                await _crimes.AddAsync(mapped);
                await _crimes.SaveChanges();
                return new OutputHandler { IsErrorOccured = false, Message = "Crime Added" };
            }
            catch (Exception)
            {
                return new OutputHandler { IsErrorOccured = true };
            }

        }

        public async Task<OutputHandler> Edit(CrimeMantainanceDTO crime)
        {
            try
            {
                var mapped = new AutoMapper<CrimeMantainanceDTO, Crimes>().MapToObject(crime);
                await _crimes.UpdateAsync(mapped);
                return new OutputHandler { IsErrorOccured = false, Message = "Crime Updated Successfully" };
            }
            catch (Exception)
            {
                return new OutputHandler { IsErrorOccured = true };
            }
        }

        public async Task<OutputHandler> GetAll()
        {
            try
            {
                var crimes = await _crimes.GetAll();
                return new OutputHandler { IsErrorOccured = false, Result = crimes };

            }
            catch (Exception)
            {

                return new OutputHandler { IsErrorOccured = true };

            }

        }

        public async Task<Crimes> GetById(int crimeId)
        {
            var crime = await _crimes.GetItem(x => x.CrimeChargeId == crimeId);
            return crime;
        }

        public async Task<OutputHandler> Delete(int crimeId)
        {
            var output = await _crimes.DeleteByIdAsync(crimeId);
            if (output.IsErrorOccured)
            {
                return new OutputHandler { IsErrorOccured = true};
            }
            else
            {
                return new OutputHandler { IsErrorOccured = false };
            }
        }
    }
}
