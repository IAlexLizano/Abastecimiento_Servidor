using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class RecipeConfig : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(e => e.IdRecipe).HasName("recipe_pk");

            builder.ToTable("recipe");

            builder.Property(e => e.IdRecipe).HasColumnName("id_recipe");
            builder.Property(e => e.CantCar).HasColumnName("cant_car");
            builder.Property(e => e.IdComponent).HasColumnName("id_component");
            builder.Property(e => e.IdModel).HasColumnName("id_model");
            builder.Property(e => e.IdStation).HasColumnName("id_station");
            builder.Property(e => e.Ownership)
                .HasDefaultValueSql("'\"CIAUTO\"'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("ownership");
            builder.Property(e => e.ProcessSheet)
                .HasColumnType("character varying")
                .HasColumnName("process_sheet");
            builder.Property(e => e.Total).HasColumnName("total");

            builder.HasOne(d => d.IdComponentNavigation).WithMany(p => p.Recipe)
                .HasForeignKey(d => d.IdComponent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_component_catalog_fk");

            builder.HasOne(d => d.IdModelNavigation).WithMany(p => p.Recipe)
                .HasForeignKey(d => d.IdModel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_model_fk");

            builder.HasOne(d => d.IdStationNavigation).WithMany(p => p.Recipe)
                .HasForeignKey(d => d.IdStation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_work_station_fk");
        }
    }
}