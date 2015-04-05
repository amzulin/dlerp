using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class PurchaseRequireMap : EntityTypeConfiguration<PurchaseRequire>
    {
        public PurchaseRequireMap()
        {
            // Primary Key
            this.HasKey(t => t.requireNo);

            // Properties
            this.Property(t => t.requireNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PurchaseRequire");
            this.Property(t => t.requireNo).HasColumnName("requireNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.orderDetailSn).HasColumnName("orderDetailSn");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.PurchaseRequires)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.PurchaseRequires)
                .HasForeignKey(d => d.staffId);

        }
    }
}
