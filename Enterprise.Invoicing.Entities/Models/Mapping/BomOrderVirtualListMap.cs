using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomOrderVirtualListMap : EntityTypeConfiguration<BomOrderVirtualList>
    {
        public BomOrderVirtualListMap()
        {
            // Primary Key
            this.HasKey(t => t.virtualSn);

            // Properties
            this.Property(t => t.remark)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BomOrderVirtualList");
            this.Property(t => t.virtualSn).HasColumnName("virtualSn");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.virtualId).HasColumnName("virtualId");
            this.Property(t => t.sAmount).HasColumnName("sAmount");
            this.Property(t => t.sPrice).HasColumnName("sPrice");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomOrderDetail)
                .WithMany(t => t.BomOrderVirtualLists)
                .HasForeignKey(d => d.detailSn);
            this.HasRequired(t => t.BomVirtual)
                .WithMany(t => t.BomOrderVirtualLists)
                .HasForeignKey(d => d.virtualId);

        }
    }
}
