using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductionDetailModelMap : EntityTypeConfiguration<V_ProductionDetailModel>
    {
        public V_ProductionDetailModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.produceNo, t.productAmount, t.productBackAmount, t.status, t.canfs, t.valid, t.isover, t.isclose, t.productRemark, t.productName, t.edittype });

            // Properties
            this.Property(t => t.produceNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productNo)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.productRemark)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.productName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.productModel)
                .HasMaxLength(50);

            this.Property(t => t.productUnit)
                .HasMaxLength(50);

            this.Property(t => t.productFast)
                .HasMaxLength(50);

            this.Property(t => t.productTu)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.edittype)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("V_ProductionDetailModel");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.productNo).HasColumnName("productNo");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.orderDetailSn).HasColumnName("orderDetailSn");
            this.Property(t => t.productAmount).HasColumnName("productAmount");
            this.Property(t => t.productBackAmount).HasColumnName("productBackAmount");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.productRemark).HasColumnName("productRemark");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.productName).HasColumnName("productName");
            this.Property(t => t.productModel).HasColumnName("productModel");
            this.Property(t => t.productUnit).HasColumnName("productUnit");
            this.Property(t => t.productFast).HasColumnName("productFast");
            this.Property(t => t.productTu).HasColumnName("productTu");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.edittype).HasColumnName("edittype");
            this.Property(t => t.xslength).HasColumnName("xslength");
        }
    }
}
