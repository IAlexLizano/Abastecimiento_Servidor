using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class PalletInspectionTeamConfig : IEntityTypeConfiguration<BoxInspectionTeam>
    {
        public void Configure(EntityTypeBuilder<BoxInspectionTeam> builder)
        {
            builder.HasKey(e => e.IdTeam).HasName("pallet_inspection_team_pkey");

            builder.ToTable("box_inspection_team");

            builder.Property(e => e.IdTeam)
                .HasDefaultValueSql("nextval('pallet_inspection_team_id_team_seq'::regclass)")
                .HasColumnName("id_team");
            builder.Property(e => e.AssignedColor)
                .HasMaxLength(50)
                .HasColumnName("assigned_color");
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            builder.Property(e => e.IdBox).HasColumnName("id_box");

            builder.HasOne(d => d.IdBoxNavigation).WithMany(p => p.BoxInspectionTeam)
                .HasForeignKey(d => d.IdBox)
                .HasConstraintName("box_inspection_team_box_fk");
        }
    }
}