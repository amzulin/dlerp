using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductPullModelMap : EntityTypeConfiguration<V_ProductPullModel>
    {
        public V_ProductPullModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.pullNo, t.makeAmount, t.pullDate, t.backDate, t.status, t.canfs, t.valid, t.isover, t.isclose, t.createDate, t.giveAmount });

            // Properties
            this.Property(t => t.pullNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.produceNo)
                .HasMaxLength(50);

            this.Property(t => t.makeAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
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

            this.Property(t => t.giveAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_ProductPullModel");
            this.Property(t => t.pullNo).HasColumnName("pullNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.makeAmount).HasColumnName("makeAmount");
            this.Property(t => t.pullDate).HasColumnName("pullDate");
            this.Property(t => t.backDate).HasColumnName("backDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.orderDetailSn).HasColumnName("orderDetailSn");
            this.Property(t => t.totalAmount).HasColumnName("totalAmount");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.hadPullAmount).HasColumnName("hadPullAmount");
            this.Property(t => t.giveAmount).HasColumnName("giveAmount");
        }
    }
}
