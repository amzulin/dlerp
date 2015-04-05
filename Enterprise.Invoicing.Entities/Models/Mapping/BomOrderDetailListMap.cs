using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomOrderDetailListMap : EntityTypeConfiguration<BomOrderDetailList>
    {
        public BomOrderDetailListMap()
        {
            // Primary Key
            this.HasKey(t => t.detailListSn);

            // Properties
            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BomOrderDetailList");
            this.Property(t => t.detailListSn).HasColumnName("detailListSn");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.bomAmount).HasColumnName("bomAmount");
            this.Property(t => t.remainAmount).HasColumnName("remainAmount");
            this.Property(t => t.needDate).HasColumnName("needDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomMain)
                .WithMany(t => t.BomOrderDetailLists)
                .HasForeignKey(d => d.bomId);
            this.HasRequired(t => t.BomOrderDetail)
                .WithMany(t => t.BomOrderDetailLists)
                .HasForeignKey(d => d.detailSn);

        }
    }
}
