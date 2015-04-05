using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockInDetailMap : EntityTypeConfiguration<StockInDetail>
    {
        public StockInDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.stockinNo)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.changeNo)
                .HasMaxLength(50);

            this.Property(t => t.purchaseNo)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockInDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.stockinNo).HasColumnName("stockinNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.inAmount).HasColumnName("inAmount");
            this.Property(t => t.changeNo).HasColumnName("changeNo");
            this.Property(t => t.purchaseNo).HasColumnName("purchaseNo");
            this.Property(t => t.purchaseDetailSn).HasColumnName("purchaseDetailSn");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Depot)
                .WithMany(t => t.StockInDetails)
                .HasForeignKey(d => d.depotId);
            this.HasRequired(t => t.Material)
                .WithMany(t => t.StockInDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasOptional(t => t.StockIn)
                .WithMany(t => t.StockInDetails)
                .HasForeignKey(d => d.stockinNo);

        }
    }
}
