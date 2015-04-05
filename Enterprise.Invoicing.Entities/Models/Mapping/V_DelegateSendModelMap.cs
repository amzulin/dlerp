using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_DelegateSendModelMap : EntityTypeConfiguration<V_DelegateSendModel>
    {
        public V_DelegateSendModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.sendNo, t.sendDate, t.needDate, t.createDate, t.status, t.canfs, t.valid, t.isover, t.isclose, t.supplierName });

            // Properties
            this.Property(t => t.sendNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_DelegateSendModel");
            this.Property(t => t.sendNo).HasColumnName("sendNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.needDate).HasColumnName("needDate");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.supplierNo).HasColumnName("supplierNo");
        }
    }
}
