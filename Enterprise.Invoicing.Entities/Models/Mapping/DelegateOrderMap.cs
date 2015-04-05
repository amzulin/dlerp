using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DelegateOrderMap : EntityTypeConfiguration<DelegateOrder>
    {
        public DelegateOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.delegateNo);

            // Properties
            this.Property(t => t.delegateNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DelegateOrder");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.orderDetailSn).HasColumnName("orderDetailSn");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.giveAmount).HasColumnName("giveAmount");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.backDate).HasColumnName("backDate");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
