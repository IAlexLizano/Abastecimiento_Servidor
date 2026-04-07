using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class AccessControlConfig : IEntityTypeConfiguration<AccessControl>
    {
        public void Configure(EntityTypeBuilder<AccessControl> builder)
        {
            builder.HasKey(e => e.AccessId).HasName("access_control_pkey");

            builder.ToTable("access_control");

            builder.HasIndex(e => new { e.RoleId, e.ServiceId }, "uq_role_service").IsUnique();

            builder.Property(e => e.AccessId).HasColumnName("access_id");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            builder.Property(e => e.RoleId).HasColumnName("role_id");
            builder.Property(e => e.ServiceId).HasColumnName("service_id");

            builder.HasOne(d => d.Role).WithMany(p => p.AccessControl)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_access_role");

            builder.HasOne(d => d.Service).WithMany(p => p.AccessControl)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("fk_access_service");
        }
    }
}