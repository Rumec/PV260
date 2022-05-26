using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class FileUrl : BaseEntity
    {
        public string Url { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime? ValidTo { get; set; }
    }
}
