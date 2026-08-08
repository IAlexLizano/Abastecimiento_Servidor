using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace PreOpeningPersistance.Context
{
    public class BaseContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<Lot> Lot { get; set; }
        public virtual DbSet<Container> Container { get; set; }
        public virtual DbSet<Pallet> Pallet { get; set; }
        public virtual DbSet<Box> Box { get; set; }
        public virtual DbSet<BoxProductDetail> BoxProductDetail { get; set; }
        public virtual DbSet<Cardboard> Cardboard{ get; set; }
        public virtual DbSet<Recipe> Recipe{ get; set; }
        public virtual DbSet<Component> Component{ get; set; }
        public virtual DbSet<Store> Store{ get; set; }
        public virtual DbSet<RegisteredUser> Users{ get; set; }
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
