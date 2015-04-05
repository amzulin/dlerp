using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DelegateOrderDetailMap : EntityTypeConfiguration<DelegateOrderDetail>
    {
        public DelegateOrderDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.delegateNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DelegateOrderDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.sendAmount).HasColumnName("sendAmount");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
