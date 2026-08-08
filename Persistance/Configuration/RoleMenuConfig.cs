using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class RoleMenuConfig : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasKey(e => new { e.IdMenu, e.IdRole }).HasName("role_menu_pkey");

            builder.ToTable("role_menu");

            builder.Property(e => e.IdMenu).HasColumnName("id_menu");
            builder.Property(e => e.IdRole).HasColumnName("id_role");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.HasOne(d => d.IdMenuNavigation).WithMany(p => p.RoleMenu)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("fk_role_menu_menu");

            builder.HasOne(d => d.IdRoleNavigation).WithMany(p => p.RoleMenu)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("fk_role_menu_role");
        }
    }
}