using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(e => e.MenuId).HasName("menu_pkey");

            builder.ToTable("menu");

            builder.Property(e => e.MenuId).HasColumnName("menu_id");
            builder.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("display_order");
            builder.Property(e => e.HasSubmenu)
                .HasDefaultValue(false)
                .HasColumnName("has_submenu");
            builder.Property(e => e.Icon)
                .HasMaxLength(100)
                .HasColumnName("icon");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            builder.Property(e => e.IsVisible)
                .HasDefaultValue(true)
                .HasColumnName("is_visible");
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            builder.Property(e => e.ParentMenuId).HasColumnName("parent_menu_id");
            builder.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");

            builder.HasOne(d => d.ParentMenu).WithMany(p => p.InverseParentMenu)
                .HasForeignKey(d => d.ParentMenuId)
                .HasConstraintName("fk_menu_parent");
        }
    }
}