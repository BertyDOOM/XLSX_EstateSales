using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLSX_EstateSales.ApplicationDbContext
{
    using DocumentFormat.OpenXml.InkML;
    using Microsoft.EntityFrameworkCore;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity;
    using System.Reflection.Emit;

    namespace XLSX_EstateSales
    {
        public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            public Microsoft.EntityFrameworkCore.DbSet<Estate> Estates { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<Town> Towns { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<Address> Addresses { get; set; }
            public Microsoft.EntityFrameworkCore.DbSet<PropertyType> PropertyTypes { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-7AG4V62;Database=XLSX_EstateSales;Trusted_Connection=true;TrustServerCertificate=true");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

            }
            public override int SaveChanges()
            {
                var entries = ChangeTracker.Entries()
                    .Where(x => x.Entity is BaseModel)
                    .ToList();

                foreach (var entry in entries)
                {
                    var model = (BaseModel)entry.Entity;

                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        model.CreatedAt = DateTime.UtcNow;
                    }
                    else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    {
                        model.UpdatedAt = DateTime.UtcNow;
                    }
                }

                return base.SaveChanges();
            }
        }
    }

}
