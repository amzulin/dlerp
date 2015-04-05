using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ReportDepotMap : EntityTypeConfiguration<V_ReportDepot>
    {
        public V_ReportDepotMap()
        {
            // Primary Key
            this.HasKey(t => new { t.depotId, t.depotName, t.valid, t.detailSn, t.depotAmount, t.depotCost, t.depotSafe, t.materialName, t.mvalid, t.materialNo, t.price });

            // Properties
            this.Property(t => t.depotId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depotName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.category)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.orderNo)
                .HasMaxLength(50);

            this.Property(t => t.mremark)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialTu)
                .HasMaxLength(50);

            this.Property(t => t.unit2)
                .HasMaxLength(50);

            this.Property(t => t.price)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_ReportDepot");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.depotName).HasColumnName("depotName");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.depotAmount).HasColumnName("depotAmount");
            this.Property(t => t.depotCost).HasColumnName("depotCost");
            this.Property(t => t.depotSafe).HasColumnName("depotSafe");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.orderNo).HasColumnName("orderNo");
            this.Property(t => t.mvalid).HasColumnName("mvalid");
            this.Property(t => t.mremark).HasColumnName("mremark");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialTu).HasColumnName("materialTu");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.oldprice).HasColumnName("oldprice");
            this.Property(t => t.price).HasColumnName("price");
        }
    }
}
