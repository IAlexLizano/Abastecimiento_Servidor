using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class StoreConfig : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(e => e.IdStore).HasName("store_pk");

            builder.ToTable("store");

            builder.Property(e => e.IdStore)
                .HasDefaultValueSql("nextval('store_store_id_seq'::regclass)")
                .HasColumnName("id_store");
            builder.Property(e => e.StoreCode)
                .HasColumnType("character varying")
                .HasColumnName("store_code");
            builder.Property(e => e.StoreName)
                .HasColumnType("character varying")
                .HasColumnName("store_name");
        }
    }
}