using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implementation
{
    public class FileUrlService : IFileUrlService
    {
        private readonly StockSystemDbContext _context;

        public FileUrlService(StockSystemDbContext context)
        {
            _context = context;
        }

        public async Task<FileUrl?> GetLatest()
        {
            return await _context.FileUrls.Where(fileUrl => fileUrl.ValidTo == null).FirstOrDefaultAsync();
        }

        public async Task<FileUrl> SetNewFileUrl(string url)
        {
            await _context.FileUrls.Where(fileUrl => fileUrl.ValidTo == null).ForEachAsync(x => x.ValidTo = DateTime.Now);
            var result = await _context.FileUrls.AddAsync(new FileUrl() { Url = url });
            await _context.SaveChangesAsync();

            result.State = EntityState.Detached;
            return result.Entity;
        }

        public async Task<List<FileUrl>> GetAll()
        {
            return await _context
                .FileUrls
                .OrderByDescending(fileUrl => fileUrl.ValidTo)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
