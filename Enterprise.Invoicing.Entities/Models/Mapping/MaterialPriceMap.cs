using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MaterialPriceMap : EntityTypeConfiguration<MaterialPrice>
    {
        public MaterialPriceMap()
        {
            // Primary Key
            this.HasKey(t => t.priceId);

            // Properties
            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("MaterialPrice");
            this.Property(t => t.priceId).HasColumnName("priceId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.startDate).HasColumnName("startDate");
            this.Property(t => t.endDate).HasColumnName("endDate");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.MaterialPrices)
                .HasForeignKey(d => d.staffId);
            this.HasRequired(t => t.Material)
                .WithMany(t => t.MaterialPrices)
                .HasForeignKey(d => d.materialNo);
            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.MaterialPrices)
                .HasForeignKey(d => d.supplierId);

        }
    }
}
