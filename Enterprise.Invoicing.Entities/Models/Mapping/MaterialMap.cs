using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MaterialMap : EntityTypeConfiguration<Material>
    {
        public MaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.materialNo);

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

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.unit2)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.orderNo)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.image)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Material");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.bigcate).HasColumnName("bigcate");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.orderNo).HasColumnName("orderNo");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.image).HasColumnName("image");
        }
    }
}
