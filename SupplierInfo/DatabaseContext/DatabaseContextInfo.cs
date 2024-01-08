using Microsoft.EntityFrameworkCore;
using SupplierInfo.Models;

namespace SupplierInfo.DatabaseContext
{
    public class DatabaseContextInfo : DbContext
    {
        public DatabaseContextInfo(DbContextOptions<DatabaseContextInfo> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
