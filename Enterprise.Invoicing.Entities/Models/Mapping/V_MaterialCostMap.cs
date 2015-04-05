using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_MaterialCostMap : EntityTypeConfiguration<V_MaterialCost>
    {
        public V_MaterialCostMap()
        {
            // Primary Key
            this.HasKey(t => new { t.materialNo, t.materialName, t.poPrice, t.createDate, t.poAmount, t.costSn });

            // Properties
            this.Property(t => t.materialNo)
                .IsRequired()
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

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.costSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_MaterialCost");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.bigcate).HasColumnName("bigcate");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.poPrice).HasColumnName("poPrice");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.poAmount).HasColumnName("poAmount");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.costSn).HasColumnName("costSn");
        }
    }
}
