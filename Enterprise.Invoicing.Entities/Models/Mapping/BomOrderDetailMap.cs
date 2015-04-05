using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomOrderDetailMap : EntityTypeConfiguration<BomOrderDetail>
    {
        public BomOrderDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.requireNo)
                .HasMaxLength(50);

            this.Property(t => t.delegateNo)
                .HasMaxLength(50);

            this.Property(t => t.produceNo)
                .HasMaxLength(50);

            this.Property(t => t.createStaff)
                .HasMaxLength(50);

            this.Property(t => t.bomSerial)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BomOrderDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.sellAmount).HasColumnName("sellAmount");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.hadRequire).HasColumnName("hadRequire");
            this.Property(t => t.haddelegate).HasColumnName("haddelegate");
            this.Property(t => t.hadproduce).HasColumnName("hadproduce");
            this.Property(t => t.requireNo).HasColumnName("requireNo");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.createStaff).HasColumnName("createStaff");
            this.Property(t => t.bomDate).HasColumnName("bomDate");
            this.Property(t => t.bomSerial).HasColumnName("bomSerial");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomMain)
                .WithMany(t => t.BomOrderDetails)
                .HasForeignKey(d => d.bomId);
            this.HasRequired(t => t.BomOrder)
                .WithMany(t => t.BomOrderDetails)
                .HasForeignKey(d => d.bomOrderNo);

        }
    }
}
