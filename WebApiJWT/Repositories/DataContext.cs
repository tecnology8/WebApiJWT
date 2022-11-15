using Microsoft.EntityFrameworkCore;
using WebApiJWT.Models;

namespace WebApiJWT.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options):base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePrivilege> RolePrivilege { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}