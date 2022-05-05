using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class EmailDoesNotExistException : Exception
    {
        public EmailDoesNotExistException(int id) : base($"Email with id ${id} does not exist.") {}
    }
}
