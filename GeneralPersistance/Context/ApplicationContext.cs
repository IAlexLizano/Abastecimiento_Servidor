using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GeneralPersistance.Context
{
    public class BaseContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<Component> Component { get; set; }

        public virtual DbSet<Recipe> Recipe { get; set; }

        public virtual DbSet<WorkStation> WorkStation { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Store> Store { get; set; }

        public virtual DbSet<Container> Container { get; set; }
        public virtual DbSet<Box> Box { get; set; }
        public virtual DbSet<Cardboard> Cardboard { get; set; }
        public virtual DbSet<Pallet> Pallet { get; set; }
        public virtual DbSet<EngineeringIssue> EngineeringIssue { get; set; }
        public virtual DbSet<RegisteredUser> RegisteredUser { get; set; }
        public virtual DbSet<Supply> Supply { get; set; }
        public virtual DbSet<Lot> Lot { get; set; }
        public virtual DbSet<BoxInspectionTeamMember> BoxInspectionTeamMember { get; set; }

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
