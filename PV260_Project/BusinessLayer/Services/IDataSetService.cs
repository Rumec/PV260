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
        List<DataSet> GetAllDataSets();
        DataSet GetDataSetById(int id);
        void CreateDataSet(DataSet dataSet);
        void DeleteDataSet(int id);
    }
}
