using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_MaterialPriceModelMap : EntityTypeConfiguration<V_MaterialPriceModel>
    {
        public V_MaterialPriceModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.priceId, t.supplierId, t.materialNo, t.price, t.startDate, t.status, t.materialName, t.xslength, t.supplierName, t.type });

            // Properties
            this.Property(t => t.priceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.supplierId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.price)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.category)
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

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_MaterialPriceModel");
            this.Property(t => t.priceId).HasColumnName("priceId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.startDate).HasColumnName("startDate");
            this.Property(t => t.endDate).HasColumnName("endDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.type).HasColumnName("type");
        }
    }
}
