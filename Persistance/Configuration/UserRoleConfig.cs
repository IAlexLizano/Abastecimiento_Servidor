using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(e => new { e.RoleId, e.UserId }).HasName("user_role_pkey");

            builder.ToTable("user_role");

            builder.Property(e => e.RoleId).HasColumnName("role_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.AssignedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("assigned_at");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            builder.HasOne(d => d.Role).WithMany(p => p.UserRole)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_user_role_role");

            builder.HasOne(d => d.User).WithMany(p => p.UserRole)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_role_user");
        }
    }
}