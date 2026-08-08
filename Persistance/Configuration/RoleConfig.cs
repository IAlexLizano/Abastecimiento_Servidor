using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.IdRole).HasName("role_pkey");

            builder.ToTable("role");

            builder.HasIndex(e => e.Name, "role_name_key").IsUnique();

            builder.Property(e => e.IdRole)
                .HasDefaultValueSql("nextval('role_role_id_seq'::regclass)")
                .HasColumnName("id_role");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        }
    }
}