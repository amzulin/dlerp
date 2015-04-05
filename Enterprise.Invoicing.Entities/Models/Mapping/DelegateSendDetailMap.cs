using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DelegateSendDetailMap : EntityTypeConfiguration<DelegateSendDetail>
    {
        public DelegateSendDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.sendNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.delegateNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("DelegateSendDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.sendNo).HasColumnName("sendNo");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.productAmount).HasColumnName("productAmount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.backProduct).HasColumnName("backProduct");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.delegateDetailSn).HasColumnName("delegateDetailSn");
            this.Property(t => t.theoryAmount).HasColumnName("theoryAmount");
            this.Property(t => t.realAmount).HasColumnName("realAmount");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.DelegateOrderDetail)
                .WithMany(t => t.DelegateSendDetails)
                .HasForeignKey(d => d.delegateDetailSn);
            this.HasRequired(t => t.DelegateSend)
                .WithMany(t => t.DelegateSendDetails)
                .HasForeignKey(d => d.sendNo);

        }
    }
}
