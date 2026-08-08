using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class AccessControlConfig : IEntityTypeConfiguration<AccessControl>
    {
        public void Configure(EntityTypeBuilder<AccessControl> builder)
        {
            builder.HasKey(e => e.IdAccess).HasName("access_control_pkey");

            builder.ToTable("access_control");

            builder.HasIndex(e => new { e.IdRole, e.IdService }, "uq_role_service").IsUnique();

            builder.Property(e => e.IdAccess)
                .HasDefaultValueSql("nextval('access_control_access_id_seq'::regclass)")
                .HasColumnName("id_access");
            builder.Property(e => e.IdRole).HasColumnName("id_role");
            builder.Property(e => e.IdService).HasColumnName("id_service");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.HasOne(d => d.IdRoleNavigation).WithMany(p => p.AccessControl)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("fk_access_role");

            builder.HasOne(d => d.IdServiceNavigation).WithMany(p => p.AccessControl)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("fk_access_service");
        }
    }
}