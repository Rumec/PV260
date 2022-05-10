using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLayer.Services
{
    public interface IDataSetService
    {
        Task<List<DataSet>> GetAllDataSets();
        Task<DataSet> GetDataSetById(int id);
        Task CreateDataSet(DataSet dataSet);
        Task DeleteDataSet(int id);
    }
}
