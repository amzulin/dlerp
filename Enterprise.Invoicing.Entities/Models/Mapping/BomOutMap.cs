using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomOutMap : EntityTypeConfiguration<BomOut>
    {
        public BomOutMap()
        {
            // Primary Key
            this.HasKey(t => t.bomOutNo);

            // Properties
            this.Property(t => t.bomOutNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BomOut");
            this.Property(t => t.bomOutNo).HasColumnName("bomOutNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomOrderDetail)
                .WithMany(t => t.BomOuts)
                .HasForeignKey(d => d.detailSn);
            this.HasRequired(t => t.Department)
                .WithMany(t => t.BomOuts)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.BomOuts)
                .HasForeignKey(d => d.staffId);

        }
    }
}
