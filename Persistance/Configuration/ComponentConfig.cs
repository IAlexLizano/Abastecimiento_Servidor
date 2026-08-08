using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ComponentConfig : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.HasKey(e => e.IdComponent).HasName("component_catalog_pkey");

            builder.ToTable("component");

            builder.HasIndex(e => e.PartCode, "component_catalog_part_code_key").IsUnique();

            builder.Property(e => e.IdComponent)
                .HasDefaultValueSql("nextval('component_catalog_id_component_seq'::regclass)")
                .HasColumnName("id_component");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.ImagePath).HasColumnName("image_path");
            builder.Property(e => e.PartCode)
                .HasMaxLength(100)
                .HasColumnName("part_code");
            builder.Property(e => e.TechParameters).HasColumnName("tech_parameters");

        }
    }
}
