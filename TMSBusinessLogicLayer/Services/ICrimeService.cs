using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface ICrimeService
    {
        Task<IEnumerable<TrafficMonitorTransactions>> GetAllTransaction(DateTime start, DateTime end, bool isDateRangeProvided);
        Task<IEnumerable<Crimes>> GetCrimes();
        Task<CrimeChargeOutputHandler> Charged(CrimeDTO crimeDTO);
        Task<IEnumerable<TrafficMonitorTransactions>> GetRegistrationCharges(string regnumber, string checkType);

    } 
}
