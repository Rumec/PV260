using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLayer.Services
{
    public interface IFileUrlService
    {
        Task<FileUrl?> GetLatest();
        Task<FileUrl> SetNewFileUrl(string url);
        Task<List<FileUrl>> GetAll();
    }
}
