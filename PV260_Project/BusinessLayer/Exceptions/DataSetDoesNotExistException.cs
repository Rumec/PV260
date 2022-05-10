using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class DataSetDoesNotExistException : Exception
    {
        public DataSetDoesNotExistException(int id) : base($"Data set with id ${id} does not exist."){ }
    }
}
