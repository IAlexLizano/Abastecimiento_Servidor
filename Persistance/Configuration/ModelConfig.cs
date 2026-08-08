using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ModelConfig : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(e => e.IdModel).HasName("model_pk");

            builder.ToTable("model");

            builder.Property(e => e.IdModel).HasColumnName("id_model");
            builder.Property(e => e.IssueDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("issue_date");
            builder.Property(e => e.ModelCode)
                .HasColumnType("character varying")
                .HasColumnName("model_code");
            builder.Property(e => e.ModelName)
                .HasColumnType("character varying")
                .HasColumnName("model_name");
            builder.Property(e => e.Version)
                .HasDefaultValue(0)
                .HasColumnName("version");
        }
    }
}
