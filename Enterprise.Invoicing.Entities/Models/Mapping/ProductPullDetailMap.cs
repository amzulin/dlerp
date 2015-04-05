using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductPullDetailMap : EntityTypeConfiguration<ProductPullDetail>
    {
        public ProductPullDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.pullSn);

            // Properties
            this.Property(t => t.pullNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductPullDetail");
            this.Property(t => t.pullSn).HasColumnName("pullSn");
            this.Property(t => t.pullNo).HasColumnName("pullNo");
            this.Property(t => t.semiId).HasColumnName("semiId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.theoryAmount).HasColumnName("theoryAmount");
            this.Property(t => t.realAmount).HasColumnName("realAmount");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
