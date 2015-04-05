using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomOrderModelMap : EntityTypeConfiguration<V_BomOrderModel>
    {
        public V_BomOrderModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.status, t.createDate, t.valid, t.isover, t.depId, t.staffId, t.bomOrderNo, t.type, t.orderType, t.canfs, t.isclose });

            // Properties
            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.orderType)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_BomOrderModel");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.orderType).HasColumnName("orderType");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.isclose).HasColumnName("isclose");
        }
    }
}
