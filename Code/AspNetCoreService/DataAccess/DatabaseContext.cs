using AspNetCoreService.CoreModel;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreService.DataAccess
{
    public sealed class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Contact> Contacts => Set<Contact>();
    }
}