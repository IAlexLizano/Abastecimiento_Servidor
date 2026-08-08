using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class BoxConfig : IEntityTypeConfiguration<Box>
    {
        public void Configure(EntityTypeBuilder<Box> builder)
            {
            builder.HasKey(e => e.IdBox).HasName("box_pkey");

            builder.ToTable("box");

            builder.Property(e => e.IdBox).HasColumnName("id_box");
            builder.Property(e => e.BoxNumber)
                .HasMaxLength(100)
                .HasColumnName("box_number");
            builder.Property(e => e.IdPallet).HasColumnName("id_pallet");
            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'RECIBIDO'::character varying")
                .HasColumnName("status");

            builder.HasOne(d => d.IdPalletNavigation).WithMany(p => p.Box)
                .HasForeignKey(d => d.IdPallet)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("box_id_pallet_fkey");
        }
    }
}