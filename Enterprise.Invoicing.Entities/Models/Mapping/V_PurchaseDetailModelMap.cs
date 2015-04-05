using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_PurchaseDetailModelMap : EntityTypeConfiguration<V_PurchaseDetailModel>
    {
        public V_PurchaseDetailModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.purchaseNo, t.supplierId, t.type, t.status, t.isover, t.createDate, t.valid, t.supplierName, t.materialNo, t.poAmount, t.poPrice, t.poRemain, t.returnAmount, t.materialName });

            // Properties
            this.Property(t => t.purchaseNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.returnNo)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.detailRemark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_PurchaseDetailModel");
            this.Property(t => t.purchaseNo).HasColumnName("purchaseNo");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.poAmount).HasColumnName("poAmount");
            this.Property(t => t.poPrice).HasColumnName("poPrice");
            this.Property(t => t.poRemain).HasColumnName("poRemain");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.returnNo).HasColumnName("returnNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.detailRemark).HasColumnName("detailRemark");
        }
    }
}
