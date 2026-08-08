using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class RegisteredUserConfig : IEntityTypeConfiguration<RegisteredUser>
    {
        public void Configure(EntityTypeBuilder<RegisteredUser> builder)
        {
            builder.HasKey(e => e.IdUser).HasName("user_account_pkey");

            builder.ToTable("registered_user");

            builder.HasIndex(e => e.Username, "user_account_username_key").IsUnique();

            builder.Property(e => e.IdUser)
                .HasDefaultValueSql("nextval('user_account_user_id_seq'::regclass)")
                .HasColumnName("id_user");
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
            builder.Property(e => e.IdStation)
                .HasDefaultValue(1)
                .HasColumnName("id_station");
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

            builder.HasOne(d => d.IdStationNavigation).WithMany(p => p.RegisteredUser)
                .HasForeignKey(d => d.IdStation)
                .HasConstraintName("registered_user_work_station_fk");
        }
    }
}