using System;
using System.Collections.Generic;
using System.Text;
using TMSDataAccessLayer.TMSDB;

namespace TMSBusinessLogicLayer.Repository
{
    public class TMSGenericRepository<T> : Repository<T> where T : class
    {
        private TMSDBContext _context;
        public TMSGenericRepository(TMSDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
