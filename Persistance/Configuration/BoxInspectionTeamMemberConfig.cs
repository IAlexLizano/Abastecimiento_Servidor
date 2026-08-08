using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class BoxInspectionTeamMemberConfig : IEntityTypeConfiguration<BoxInspectionTeamMember>
    {
        public void Configure(EntityTypeBuilder<BoxInspectionTeamMember> builder)
        {
            builder.HasKey(e => e.IdTeamMember).HasName("box_inspection_team_member_pk");

            builder.ToTable("box_inspection_team_member");

            builder.Property(e => e.IdTeamMember).HasColumnName("id_team_member");
            builder.Property(e => e.AssignedColor)
                .HasColumnType("character varying")
                .HasColumnName("assigned_color");
            builder.Property(e => e.IdTeam).HasColumnName("id_team");
            builder.Property(e => e.IdUser).HasColumnName("id_user");

            builder.HasOne(d => d.IdTeamNavigation).WithMany(p => p.BoxInspectionTeamMember)
                .HasForeignKey(d => d.IdTeam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("box_inspection_team_member_box_inspection_team_fk");

            builder.HasOne(d => d.IdUserNavigation).WithMany(p => p.BoxInspectionTeamMember)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("box_inspection_team_member_registered_user_fk");
        }
    }
}
