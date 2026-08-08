using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Persistance.Configuration
{
    public class WorkStationConfig : IEntityTypeConfiguration<WorkStation>
    {
        public void Configure(EntityTypeBuilder<WorkStation> builder)
        {
            builder.HasKey(e => e.IdStation).HasName("work_station_pkey");

            builder.ToTable("work_station");

            builder.HasIndex(e => e.Name, "work_station_name_key").IsUnique();

            builder.Property(e => e.IdStation).HasColumnName("id_station");
            builder.Property(e => e.CodeStation).HasColumnName("code_station");
            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        }
    }
}