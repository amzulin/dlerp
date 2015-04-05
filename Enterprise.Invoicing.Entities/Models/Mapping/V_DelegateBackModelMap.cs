using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_DelegateBackModelMap : EntityTypeConfiguration<V_DelegateBackModel>
    {
        public V_DelegateBackModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.backNo, t.sendNo, t.backDate, t.createDate, t.status, t.canfs, t.vaild, t.isover, t.isclose, t.supplierId, t.supplierName, t.sendDate, t.needDate });

            // Properties
            this.Property(t => t.backNo)
                .IsRequired()
                .HasMaxLength(50);

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

            this.Property(t => t.supplierId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_DelegateBackModel");
            this.Property(t => t.backNo).HasColumnName("backNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.sendNo).HasColumnName("sendNo");
            this.Property(t => t.backDate).HasColumnName("backDate");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.vaild).HasColumnName("vaild");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.supplierNo).HasColumnName("supplierNo");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.needDate).HasColumnName("needDate");
        }
    }
}
