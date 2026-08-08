using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ContainerConfig : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.HasKey(e => e.IdContainer).HasName("container_pkey");

            builder.ToTable("container");

            builder.Property(e => e.IdContainer).HasColumnName("id_container");
            builder.Property(e => e.ContainerNumber)
                .HasMaxLength(100)
                .HasColumnName("container_number");
            builder.Property(e => e.DisembarkationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("disembarkation_date");
            builder.Property(e => e.IdLot).HasColumnName("id_lot");
            builder.Property(e => e.IdStore).HasColumnName("id_store");
            builder.Property(e => e.StampNumber)
                .HasColumnType("character varying")
                .HasColumnName("stamp_number");
            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'RECIBIDO'::character varying")
                .HasColumnName("status");

            builder.HasOne(d => d.IdLotNavigation).WithMany(p => p.Container)
                .HasForeignKey(d => d.IdLot)
                .HasConstraintName("container_id_lot_fkey");

            builder.HasOne(d => d.IdStoreNavigation).WithMany(p => p.Container)
                .HasForeignKey(d => d.IdStore)
                .HasConstraintName("container_store_fk");
        }
    }
}