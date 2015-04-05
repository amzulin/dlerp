using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomProductMap : EntityTypeConfiguration<V_BomProduct>
    {
        public V_BomProductMap()
        {
            // Primary Key
            this.HasKey(t => new { t.bomId, t.priceId, t.price, t.materialNo, t.materialName, t.supplierId, t.supplierName, t.status });

            // Properties
            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.priceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.price)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.supplierId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.supplierNo)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.version)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_BomProduct");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.priceId).HasColumnName("priceId");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierNo).HasColumnName("supplierNo");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.version).HasColumnName("version");
        }
    }
}
