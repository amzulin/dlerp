using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class PurchaseDetailMap : EntityTypeConfiguration<PurchaseDetail>
    {
        public PurchaseDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.purchaseNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.requireNo)
                .HasMaxLength(50);

            this.Property(t => t.returnNo)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PurchaseDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.purchaseNo).HasColumnName("purchaseNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.requireNo).HasColumnName("requireNo");
            this.Property(t => t.requireDetailSn).HasColumnName("requireDetailSn");
            this.Property(t => t.poAmount).HasColumnName("poAmount");
            this.Property(t => t.poPrice).HasColumnName("poPrice");
            this.Property(t => t.poRemain).HasColumnName("poRemain");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.returnNo).HasColumnName("returnNo");
            this.Property(t => t.returnDetailSn).HasColumnName("returnDetailSn");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.sendDate).HasColumnName("sendDate");

            // Relationships
            this.HasRequired(t => t.Material)
                .WithMany(t => t.PurchaseDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasRequired(t => t.Purchase)
                .WithMany(t => t.PurchaseDetails)
                .HasForeignKey(d => d.purchaseNo);

        }
    }
}
