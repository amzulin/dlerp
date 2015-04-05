using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomOrderMap : EntityTypeConfiguration<BomOrder>
    {
        public BomOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.bomOrderNo);

            // Properties
            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.orderType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BomOrder");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.orderType).HasColumnName("orderType");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.BomOrders)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.BomOrders)
                .HasForeignKey(d => d.staffId);
            this.HasOptional(t => t.Supplier)
                .WithMany(t => t.BomOrders)
                .HasForeignKey(d => d.supplierId);

        }
    }
}
