using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ServiceConfig : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(e => e.IdService).HasName("service_pkey");

            builder.ToTable("service");

            builder.Property(e => e.IdService)
                .HasDefaultValueSql("nextval('service_service_id_seq'::regclass)")
                .HasColumnName("id_service");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            builder.Property(e => e.ServiceKey)
                .HasMaxLength(500)
                .HasColumnName("service_key");
        }
    }
}