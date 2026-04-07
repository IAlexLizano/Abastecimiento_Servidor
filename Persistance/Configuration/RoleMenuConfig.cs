using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class RoleMenuConfig : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasKey(e => new { e.MenuId, e.RoleId }).HasName("role_menu_pkey");

            builder.ToTable("role_menu");

            builder.Property(e => e.MenuId).HasColumnName("menu_id");
            builder.Property(e => e.RoleId).HasColumnName("role_id");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.HasOne(d => d.Menu).WithMany(p => p.RoleMenu)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("fk_role_menu_menu");

            builder.HasOne(d => d.Role).WithMany(p => p.RoleMenu)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_role_menu_role");
        }
    }
}