using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_StockInPurchaseMap : EntityTypeConfiguration<V_StockInPurchase>
    {
        public V_StockInPurchaseMap()
        {
            // Primary Key
            this.HasKey(t => new { t.stockInNo, t.createDate, t.materialNo, t.inAmount, t.purchaseDetailSn, t.poAmount, t.poPrice, t.supplierName, t.materialName, t.returnAmount, t.inCost, t.poCost, t.returnCost });

            // Properties
            this.Property(t => t.stockInNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.purchaseNo)
                .HasMaxLength(50);

            this.Property(t => t.purchaseDetailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.category)
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.person)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_StockInPurchase");
            this.Property(t => t.stockInNo).HasColumnName("stockInNo");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.inAmount).HasColumnName("inAmount");
            this.Property(t => t.purchaseNo).HasColumnName("purchaseNo");
            this.Property(t => t.purchaseDetailSn).HasColumnName("purchaseDetailSn");
            this.Property(t => t.poAmount).HasColumnName("poAmount");
            this.Property(t => t.poPrice).HasColumnName("poPrice");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.person).HasColumnName("person");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.inCost).HasColumnName("inCost");
            this.Property(t => t.poCost).HasColumnName("poCost");
            this.Property(t => t.returnCost).HasColumnName("returnCost");
        }
    }
}
