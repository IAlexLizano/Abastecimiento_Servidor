using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TestPersistance.Context
{
    public class BaseContext(DbContextOptions options) : DbContext(options)
    {
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
