using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(e => new { e.IdRole, e.IdUser }).HasName("user_role_pkey");

            builder.ToTable("user_role");

            builder.Property(e => e.IdRole).HasColumnName("id_role");
            builder.Property(e => e.IdUser).HasColumnName("id_user");
            builder.Property(e => e.AssignedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("assigned_at");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.HasOne(d => d.IdRoleNavigation).WithMany(p => p.UserRole)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("fk_user_role_role");

            builder.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserRole)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("fk_user_role_user");
        }
    }
}