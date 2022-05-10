using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Exceptions;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementation
{
    public class UserEmailService : IUserEmailService
    {
        private readonly StockSystemDbContext _context;
        private readonly ILogger<UserEmailService> _logger;

        public UserEmailService(StockSystemDbContext context, ILogger<UserEmailService> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task RegisterNewEmail(string emailAddress) {
            await _context.Emails.AddAsync(new Email() { Address = emailAddress });
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Email address '{emailAddress}' was registered.");
        }

        public async Task<List<Email>> GetAllRegisteredEmails() {
            return await _context.Emails.AsNoTracking().ToListAsync();
        }

        public async Task RemoveEmail(int id) {
            var email = await _context.Emails.FindAsync(id);
            if (email == null) {
                _logger.LogError($"Email with id {id} does not exist.");
                throw new EmailDoesNotExistException(id);
            }
            
            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();
        }
    }
}
