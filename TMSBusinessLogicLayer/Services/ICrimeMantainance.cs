using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMSBusinessLogicLayer.DTOs;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Services
{
    public interface ICrimeMantainance
    {
        Task<OutputHandler> Create(CrimeMantainanceDTO crime);
        Task<OutputHandler> Delete(int crimeId);
        Task<OutputHandler> Edit(CrimeMantainanceDTO crime);
        Task<Crimes> GetById(int crimeId);
        Task<OutputHandler> GetAll();
    }
}
