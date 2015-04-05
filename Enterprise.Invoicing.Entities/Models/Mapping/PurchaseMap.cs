using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class PurchaseMap : EntityTypeConfiguration<Purchase>
    {
        public PurchaseMap()
        {
            // Primary Key
            this.HasKey(t => t.purchaseNo);

            // Properties
            this.Property(t => t.purchaseNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Purchase");
            this.Property(t => t.purchaseNo).HasColumnName("purchaseNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.totalAmount).HasColumnName("totalAmount");
            this.Property(t => t.totalCost).HasColumnName("totalCost");
            this.Property(t => t.totalRemain).HasColumnName("totalRemain");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.Purchases)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.Purchases)
                .HasForeignKey(d => d.staffId);
            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.Purchases)
                .HasForeignKey(d => d.supplierId);

        }
    }
}
