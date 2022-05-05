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
    public class UserEmailService : IUserEmailService
    {
        private readonly StockSystemDbContext _context;

        public UserEmailService(StockSystemDbContext context) {
            _context = context;
        }

        public async Task RegisterNewEmail(string emailAddress) {
            await _context.Emails.AddAsync(new Email() { Address = emailAddress });
            await _context.SaveChangesAsync();
        }

        public async Task<List<Email>> GetAllRegisteredEmails() {
            return await _context.Emails.AsNoTracking().ToListAsync();
        }

        public async Task RemoveEmail(int id) {
            var email = await _context.Emails.FindAsync(id);
            if (email == null)
                throw new EmailDoesNotExistException(id);

            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();
        }
    }
}
