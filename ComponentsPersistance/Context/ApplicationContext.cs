using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ComponentsPersistance.Context
{
    public class BaseContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<Component> Component { get; set; }

        public virtual DbSet<Recipe> Recipe { get; set; }

        public virtual DbSet<WorkStation> WorkStation { get; set; }
        public virtual DbSet<Model> Models { get; set; }

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
