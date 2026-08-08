using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace OpeningPersistance.Context
{
    public class BaseContext(DbContextOptions options) : DbContext(options)
    {
        public virtual DbSet<Box> Box { get; set; }
        public virtual DbSet<Container> Container { get; set; }
        public virtual DbSet<Lot> Lot { get; set; }
        public virtual DbSet<Pallet> Pallet { get; set; }
        public virtual DbSet<RegisteredUser> UserAccount { get; set; }
        public virtual DbSet<BoxInspectionTeam> BoxInspectionTeam { get; set; }
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
