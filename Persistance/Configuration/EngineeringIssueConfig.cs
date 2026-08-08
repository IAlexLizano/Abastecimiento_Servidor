using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class EngineeringIssueConfig : IEntityTypeConfiguration<EngineeringIssue>
    {
        public void Configure(EntityTypeBuilder<EngineeringIssue> builder)
        {
            builder.HasKey(e => e.IdIssue).HasName("engineering_issue_pkey");

            builder.ToTable("engineering_issue");

            builder.Property(e => e.IdIssue).HasColumnName("id_issue");
            builder.Property(e => e.IdDetail).HasColumnName("id_detail");
            builder.Property(e => e.IdUserReports).HasColumnName("id_user_reports");
            builder.Property(e => e.IdUserSolves).HasColumnName("id_user_solves");
            builder.Property(e => e.Reason).HasColumnName("reason");
            builder.Property(e => e.ReportDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("report_date");
            builder.Property(e => e.ResolutionStatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'PENDIENTE'::character varying")
                .HasColumnName("resolution_status");
            builder.Property(e => e.SolveDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("solve_date");

            builder.HasOne(d => d.IdDetailNavigation).WithMany(p => p.EngineeringIssue)
                .HasForeignKey(d => d.IdDetail)
                .HasConstraintName("engineering_issue_box_product_detail_fk");

            builder.HasOne(d => d.IdUserReportsNavigation).WithMany(p => p.EngineeringIssueIdUserReportsNavigation)
                .HasForeignKey(d => d.IdUserReports)
                .HasConstraintName("engineering_issue_id_reported_by_user_fkey");

            builder.HasOne(d => d.IdUserSolvesNavigation).WithMany(p => p.EngineeringIssueIdUserSolvesNavigation)
                .HasForeignKey(d => d.IdUserSolves)
                .HasConstraintName("engineering_issue_registered_user_fk");
        }
    }
}