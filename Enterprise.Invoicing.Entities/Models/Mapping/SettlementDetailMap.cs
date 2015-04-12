using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class SettlementDetailMap : EntityTypeConfiguration<SettlementDetail>
    {
        public SettlementDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.settleNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.stockNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.fromNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialUnit)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialTu)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.depotName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.stockdetailRemark)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SettlementDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.settleNo).HasColumnName("settleNo");
            this.Property(t => t.stockNo).HasColumnName("stockNo");
            this.Property(t => t.tradeDate).HasColumnName("tradeDate");
            this.Property(t => t.tradeType).HasColumnName("tradeType");
            this.Property(t => t.fromNo).HasColumnName("fromNo");
            this.Property(t => t.fromDetailSn).HasColumnName("fromDetailSn");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.materialUnit).HasColumnName("materialUnit");
            this.Property(t => t.materialTu).HasColumnName("materialTu");
            this.Property(t => t.stockdetailsn).HasColumnName("stockdetailsn");
            this.Property(t => t.tradeAmount).HasColumnName("tradeAmount");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.tradePrice).HasColumnName("tradePrice");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.depotName).HasColumnName("depotName");
            this.Property(t => t.stockdetailRemark).HasColumnName("stockdetailRemark");
            this.Property(t => t.isSettle).HasColumnName("isSettle");
            this.Property(t => t.capitalSn).HasColumnName("capitalSn");
        }
    }
}
