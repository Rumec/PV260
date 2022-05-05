using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLayer.Services
{
    public interface IUserEmailService
    {
        Task RegisterNewEmail(string emailAddress);
        Task<List<Email>> GetAllRegisteredEmails();
        Task RemoveEmail(int id);
    }
}
