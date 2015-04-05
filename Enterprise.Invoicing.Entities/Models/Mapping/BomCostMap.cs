using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomCostMap : EntityTypeConfiguration<BomCost>
    {
        public BomCostMap()
        {
            // Primary Key
            this.HasKey(t => t.costId);

            // Properties
            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BomCost");
            this.Property(t => t.costId).HasColumnName("costId");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomMain)
                .WithMany(t => t.BomCosts)
                .HasForeignKey(d => d.bomId);

        }
    }
}
