using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class CardboardConfig : IEntityTypeConfiguration<Cardboard>
    {
        public void Configure(EntityTypeBuilder<Cardboard> builder)
        {
            builder.HasKey(e => e.IdCardboard).HasName("box_product_detail_pkey");

            builder.ToTable("cardboard");

            builder.Property(e => e.IdCardboard)
                .HasDefaultValueSql("nextval('box_product_detail_id_detail_seq'::regclass)")
                .HasColumnName("id_cardboard");
            builder.Property(e => e.IdBox).HasColumnName("id_box");
            builder.Property(e => e.IdRecipe).HasColumnName("id_recipe");
            builder.Property(e => e.IdUser).HasColumnName("id_user");
            builder.Property(e => e.OpeningDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("opening_date");
            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'IN_PROGRESS'::character varying")
                .HasColumnName("status");

            builder.HasOne(d => d.IdBoxNavigation).WithMany(p => p.Cardboard)
                .HasForeignKey(d => d.IdBox)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cardboard_box_product_detail_fk");

            builder.HasOne(d => d.IdRecipeNavigation).WithMany(p => p.Cardboard)
                .HasForeignKey(d => d.IdRecipe)
                .HasConstraintName("cardboard_recipe_fk");

            builder.HasOne(d => d.IdUserNavigation).WithMany(p => p.Cardboard)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("labeled_box_registered_user_fk");
        }
    }
}
