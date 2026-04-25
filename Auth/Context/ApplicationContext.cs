using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auth.Context
{
    public class BaseContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<RoleMenu> RoleMenu { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<AccessControl> AccessControl { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : BaseContext(options)
    {
    }
}