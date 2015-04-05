using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_DelegateOrderDetailMap : EntityTypeConfiguration<V_DelegateOrderDetail>
    {
        public V_DelegateOrderDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.delegateNo, t.productNo, t.productAmount, t.productPrice, t.status, t.valid, t.isover, t.createDate, t.backDate, t.productName, t.productBackAmount, t.isclose, t.canfs, t.productGiveAmount });

            // Properties
            this.Property(t => t.delegateNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.productPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            this.Property(t => t.person)
                .HasMaxLength(50);

            this.Property(t => t.productName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productModel)
                .HasMaxLength(50);

            this.Property(t => t.productUnit)
                .HasMaxLength(50);

            this.Property(t => t.productPinyin)
                .HasMaxLength(50);

            this.Property(t => t.ProductTu)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.detailRemark)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.unit2)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.productBackAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.productGiveAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_DelegateOrderDetail");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.productNo).HasColumnName("productNo");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.productAmount).HasColumnName("productAmount");
            this.Property(t => t.productPrice).HasColumnName("productPrice");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.backDate).HasColumnName("backDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.person).HasColumnName("person");
            this.Property(t => t.productName).HasColumnName("productName");
            this.Property(t => t.productModel).HasColumnName("productModel");
            this.Property(t => t.productUnit).HasColumnName("productUnit");
            this.Property(t => t.productPinyin).HasColumnName("productPinyin");
            this.Property(t => t.ProductTu).HasColumnName("ProductTu");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.detailAmount).HasColumnName("detailAmount");
            this.Property(t => t.detailPrice).HasColumnName("detailPrice");
            this.Property(t => t.sendAmount).HasColumnName("sendAmount");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.detailRemark).HasColumnName("detailRemark");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.productBackAmount).HasColumnName("productBackAmount");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.orderDetailSn).HasColumnName("orderDetailSn");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.productGiveAmount).HasColumnName("productGiveAmount");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
        }
    }
}
