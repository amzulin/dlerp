using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class PurchaseRequireDetailMap : EntityTypeConfiguration<PurchaseRequireDetail>
    {
        public PurchaseRequireDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.requireNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PurchaseRequireDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.requireNo).HasColumnName("requireNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.orderAmount).HasColumnName("orderAmount");
            this.Property(t => t.buyAmount).HasColumnName("buyAmount");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Material)
                .WithMany(t => t.PurchaseRequireDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasRequired(t => t.PurchaseRequire)
                .WithMany(t => t.PurchaseRequireDetails)
                .HasForeignKey(d => d.requireNo);

        }
    }
}
