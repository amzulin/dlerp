using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_PurchaseModelMap : EntityTypeConfiguration<V_PurchaseModel>
    {
        public V_PurchaseModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.purchaseNo, t.staffId, t.depId, t.supplierId, t.type, t.totalAmount, t.totalCost, t.totalRemain, t.status, t.isover, t.createDate, t.valid });

            // Properties
            this.Property(t => t.purchaseNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.supplierId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_PurchaseModel");
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
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
        }
    }
}
