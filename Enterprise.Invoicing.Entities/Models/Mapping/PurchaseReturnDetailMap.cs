using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class PurchaseReturnDetailMap : EntityTypeConfiguration<PurchaseReturnDetail>
    {
        public PurchaseReturnDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.returnNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.purchaseNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.stockinNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PurchaseReturnDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.returnNo).HasColumnName("returnNo");
            this.Property(t => t.purchaseNo).HasColumnName("purchaseNo");
            this.Property(t => t.purchaseDetailSn).HasColumnName("purchaseDetailSn");
            this.Property(t => t.stockinNo).HasColumnName("stockinNo");
            this.Property(t => t.stockinDetailSn).HasColumnName("stockinDetailSn");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.buyPrice).HasColumnName("buyPrice");
            this.Property(t => t.returnCost).HasColumnName("returnCost");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Depot)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.depotId);
            this.HasRequired(t => t.Material)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasRequired(t => t.Purchase)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.purchaseNo);
            this.HasRequired(t => t.PurchaseDetail)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.purchaseDetailSn);
            this.HasRequired(t => t.PurchaseReturn)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.returnNo);
            this.HasRequired(t => t.StockIn)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.stockinNo);
            this.HasRequired(t => t.StockInDetail)
                .WithMany(t => t.PurchaseReturnDetails)
                .HasForeignKey(d => d.stockinDetailSn);

        }
    }
}
