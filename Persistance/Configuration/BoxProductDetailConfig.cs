using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class BoxProductDetailConfig : IEntityTypeConfiguration<BoxProductDetail>
    {
        public void Configure(EntityTypeBuilder<BoxProductDetail> builder)
        {
            builder.HasKey(e => e.IdBoxProductDetail).HasName("box_product_detail_pk");

            builder.ToTable("box_product_detail");

            builder.Property(e => e.IdBoxProductDetail).HasColumnName("id_box_product_detail");
            builder.Property(e => e.IdBox).HasColumnName("id_box");
            builder.Property(e => e.IdComponent).HasColumnName("id_component");
            builder.Property(e => e.PackingQuantity).HasColumnName("packing_quantity");
            builder.Property(e => e.RevisedQuantity).HasColumnName("revised_quantity");
            builder.Property(e => e.TotalQuantity).HasColumnName("total_quantity");
            builder.Property(e => e.UpdateDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("update_date");

            builder.HasOne(d => d.IdBoxNavigation).WithMany(p => p.BoxProductDetail)
                .HasForeignKey(d => d.IdBox)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("box_product_detail_box_fk");

            builder.HasOne(d => d.IdComponentNavigation).WithMany(p => p.BoxProductDetail)
                .HasForeignKey(d => d.IdComponent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("box_product_detail_component_catalog_fk");
        }
    }
}