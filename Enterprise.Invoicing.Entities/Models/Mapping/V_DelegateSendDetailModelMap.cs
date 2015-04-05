using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_DelegateSendDetailModelMap : EntityTypeConfiguration<V_DelegateSendDetailModel>
    {
        public V_DelegateSendDetailModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.detailSn, t.sendNo, t.delegateNo, t.productAmount, t.price, t.backProduct, t.bomId, t.delegateDetailSn, t.theoryAmount, t.realAmount, t.productNo, t.productName, t.materialNo, t.materialName, t.xslength });

            // Properties
            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.sendNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.delegateNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.price)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.backProduct)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.delegateDetailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.theoryAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.realAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(100);

            this.Property(t => t.productNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productModel)
                .HasMaxLength(50);

            this.Property(t => t.productUnit)
                .HasMaxLength(50);

            this.Property(t => t.productTu)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
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

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_DelegateSendDetailModel");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.sendNo).HasColumnName("sendNo");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.productAmount).HasColumnName("productAmount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.backProduct).HasColumnName("backProduct");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.delegateDetailSn).HasColumnName("delegateDetailSn");
            this.Property(t => t.theoryAmount).HasColumnName("theoryAmount");
            this.Property(t => t.realAmount).HasColumnName("realAmount");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.productNo).HasColumnName("productNo");
            this.Property(t => t.productName).HasColumnName("productName");
            this.Property(t => t.productModel).HasColumnName("productModel");
            this.Property(t => t.productUnit).HasColumnName("productUnit");
            this.Property(t => t.productTu).HasColumnName("productTu");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.materialprice).HasColumnName("materialprice");
        }
    }
}
