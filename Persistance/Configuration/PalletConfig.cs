using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class PalletConfig : IEntityTypeConfiguration<Pallet>
    {
        public void Configure(EntityTypeBuilder<Pallet> builder)
            {
            builder.HasKey(e => e.IdPallet).HasName("pallet_pkey");

            builder.ToTable("pallet");

            builder.Property(e => e.IdPallet).HasColumnName("id_pallet");
            builder.Property(e => e.Content)
                .HasColumnType("character varying")
                .HasColumnName("content");
            builder.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            builder.Property(e => e.IdContainer).HasColumnName("id_container");
            builder.Property(e => e.IdUser).HasColumnName("id_user");
            builder.Property(e => e.NeedClaim)
                .HasDefaultValue(false)
                .HasColumnName("need_claim");
            builder.Property(e => e.PalletNumber)
                .HasMaxLength(100)
                .HasColumnName("pallet_number");
            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'RECIBIDO'::character varying")
                .HasColumnName("status");

            builder.HasOne(d => d.IdContainerNavigation).WithMany(p => p.Pallet)
                .HasForeignKey(d => d.IdContainer)
                .HasConstraintName("pallet_id_container_fkey");

            builder.HasOne(d => d.IdUserNavigation).WithMany(p => p.Pallet)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("pallet_registered_user_fk");
        }
    }
}