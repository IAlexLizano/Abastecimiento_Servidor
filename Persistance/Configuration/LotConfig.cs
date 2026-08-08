using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class LotConfig : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder.HasKey(e => e.IdLot).HasName("ckd_lot_pkey");

            builder.ToTable("lot");

            builder.Property(e => e.IdLot)
                .HasDefaultValueSql("nextval('ckd_lot_id_lot_seq'::regclass)")
                .HasColumnName("id_lot");
            builder.Property(e => e.ArrivalDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("arrival_date");
            builder.Property(e => e.LotCode)
                .HasMaxLength(50)
                .HasColumnName("lot_code");
            builder.Property(e => e.Product)
                .HasMaxLength(100)
                .HasColumnName("product");
            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'RECIBIDO'::character varying")
                .HasColumnName("status");
        }
    }
}
