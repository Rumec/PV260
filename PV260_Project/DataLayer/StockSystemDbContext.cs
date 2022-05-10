using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class StockSystemDbContext : DbContext
    {
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<Email> Emails { get; set; }

        public StockSystemDbContext(DbContextOptions<StockSystemDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DataSet>()
                .HasMany(x => x.Holdings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
