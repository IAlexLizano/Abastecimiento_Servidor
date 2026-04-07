using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class UserAccountConfig : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasKey(e => e.UserId).HasName("user_account_pkey");

            builder.ToTable("user_account");

            builder.HasIndex(e => e.Username, "user_account_username_key").IsUnique();

            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("created_at");
            builder.Property(e => e.Email)
                .HasMaxLength(300)
                .HasColumnName("email");
            builder.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            builder.Property(e => e.IdCard)
                .HasMaxLength(13)
                .HasColumnName("id_card");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            builder.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            builder.Property(e => e.LoginAttempts)
                .HasDefaultValue(0)
                .HasColumnName("login_attempts");
            builder.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .HasColumnName("password_hash");
            builder.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
            builder.Property(e => e.Username)
                .HasMaxLength(500)
                .HasColumnName("username");
        }
    }
}