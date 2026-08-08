using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class SupplyConfig : IEntityTypeConfiguration<Supply>
    {
        public void Configure(EntityTypeBuilder<Supply> builder)
        {
            builder.HasKey(e => e.IdSupply).HasName("supply_kardex_pkey");

            builder.ToTable("supply");

            builder.Property(e => e.IdSupply)
                .HasDefaultValueSql("nextval('supply_kardex_id_kardex_seq'::regclass)")
                .HasColumnName("id_supply");
            builder.Property(e => e.IdCardboard).HasColumnName("id_cardboard");
            builder.Property(e => e.IdUserReceive).HasColumnName("id_user_receive");
            builder.Property(e => e.IdUserSend).HasColumnName("id_user_send");
            builder.Property(e => e.ReceiveDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("receive_date");
            builder.Property(e => e.SendDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("send_date");
            builder.Property(e => e.SupplyStatus)
                .HasColumnType("character varying")
                .HasColumnName("supply_status");

            builder.HasOne(d => d.IdCardboardNavigation).WithMany(p => p.Supply)
                .HasForeignKey(d => d.IdCardboard)
                .HasConstraintName("supply_labeled_box_fk");

            builder.HasOne(d => d.IdUserReceiveNavigation).WithMany(p => p.SupplyIdUserReceiveNavigation)
                .HasForeignKey(d => d.IdUserReceive)
                .HasConstraintName("supply_registered_user_fk");

            builder.HasOne(d => d.IdUserSendNavigation).WithMany(p => p.SupplyIdUserSendNavigation)
                .HasForeignKey(d => d.IdUserSend)
                .HasConstraintName("supply_kardex_id_user_fkey");
        }
    }
}
