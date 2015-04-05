using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_StockOutPurchaseMap : EntityTypeConfiguration<V_StockOutPurchase>
    {
        public V_StockOutPurchaseMap()
        {
            // Primary Key
            this.HasKey(t => new { t.bomOrderNo, t.supplierName, t.Amount, t.Price, t.sellAmount, t.materialNo, t.materialName, t.xslength, t.createDate, t.status, t.isover, t.depotId, t.outAmount, t.returnAmount, t.outPrice, t.orderSn, t.outDetailSn, t.totalCost, t.outCost, t.returnCost });

            // Properties
            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.category)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.stockoutNo)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depotId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.orderSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.outDetailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_StockOutPurchase");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.sellAmount).HasColumnName("sellAmount");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.stockoutNo).HasColumnName("stockoutNo");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.outPrice).HasColumnName("outPrice");
            this.Property(t => t.orderSn).HasColumnName("orderSn");
            this.Property(t => t.outDetailSn).HasColumnName("outDetailSn");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.totalCost).HasColumnName("totalCost");
            this.Property(t => t.outCost).HasColumnName("outCost");
            this.Property(t => t.returnCost).HasColumnName("returnCost");
        }
    }
}
