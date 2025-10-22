using Microsoft.EntityFrameworkCore;
using ECommerceStore.Models;

namespace ECommerceStore.Data
{
    public class DatabaseContext : DbContext
    {
        // here : base(options) --> simply means call constructor of DbContext [parent class] and pass options to it.
        // DbContextOptions<DatabaseContext> options --> DbContextOptions configuration for DatabaseContext type
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
