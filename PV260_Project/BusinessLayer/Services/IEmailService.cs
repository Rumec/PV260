using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLayer.Services
{
    public interface IEmailService
    {
        void RegisterNewEmail(string emailAddress);
        List<Email> GetAllRegisteredEmails();
        void RemoveEmail(int id);
    }
}
