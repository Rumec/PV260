using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Exceptions;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Implementation
{
    public class DataSetService : IDataSetService
    {
        private readonly StockSystemDbContext _context;

        public DataSetService(StockSystemDbContext context) {
            _context = context;
        }

        public async Task<List<DataSet>> GetAllDataSets() {
            return await _context.DataSets
                .OrderByDescending(x => x.Date)
                .Include(x => x.Holdings)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DataSet> GetDataSetById(int id) {
            var dataSet = await _context.DataSets
                .Include(x => x.Holdings)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dataSet == null)
                throw new DataSetDoesNotExistException(id);

            return dataSet;
        }

        public async Task CreateDataSet(DataSet dataSet) {
            await _context.DataSets.AddAsync(dataSet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDataSet(int id) {
            var dataSet = await _context.DataSets.FindAsync(id);
            if (dataSet == null)
                throw new DataSetDoesNotExistException(id);

            _context.DataSets.Remove(dataSet);
            await _context.SaveChangesAsync();
        }
    }
}
