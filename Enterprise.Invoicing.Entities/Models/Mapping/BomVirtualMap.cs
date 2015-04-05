using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomVirtualMap : EntityTypeConfiguration<BomVirtual>
    {
        public BomVirtualMap()
        {
            // Primary Key
            this.HasKey(t => t.virtualId);

            // Properties
            this.Property(t => t.virtualName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BomVirtual");
            this.Property(t => t.virtualId).HasColumnName("virtualId");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.virtualName).HasColumnName("virtualName");
            this.Property(t => t.vAmount).HasColumnName("vAmount");
            this.Property(t => t.vPrice).HasColumnName("vPrice");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomMain)
                .WithMany(t => t.BomVirtuals)
                .HasForeignKey(d => d.bomId);

        }
    }
}
