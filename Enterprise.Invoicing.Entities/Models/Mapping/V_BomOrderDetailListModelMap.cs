using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomOrderDetailListModelMap : EntityTypeConfiguration<V_BomOrderDetailListModel>
    {
        public V_BomOrderDetailListModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.detailListSn, t.detailSn, t.bomId, t.bomAmount, t.bomOrderNo, t.Price, t.outAmount, t.Amount, t.materialName, t.materialNo, t.forbomId, t.xslength });

            // Properties
            this.Property(t => t.detailListSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.createStaff)
                .HasMaxLength(50);

            this.Property(t => t.materialCate)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.bigcate)
                .HasMaxLength(50);

            this.Property(t => t.category)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.forbomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_BomOrderDetailListModel");
            this.Property(t => t.detailListSn).HasColumnName("detailListSn");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.bomAmount).HasColumnName("bomAmount");
            this.Property(t => t.needDate).HasColumnName("needDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.createStaff).HasColumnName("createStaff");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.materialCate).HasColumnName("materialCate");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.bigcate).HasColumnName("bigcate");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.forbomId).HasColumnName("forbomId");
            this.Property(t => t.xslength).HasColumnName("xslength");
        }
    }
}
