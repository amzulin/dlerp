using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductGiveDetailMap : EntityTypeConfiguration<ProductGiveDetail>
    {
        public ProductGiveDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.giveSn);

            // Properties
            this.Property(t => t.giveNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductGiveDetail");
            this.Property(t => t.giveSn).HasColumnName("giveSn");
            this.Property(t => t.giveNo).HasColumnName("giveNo");
            this.Property(t => t.pullDetailSn).HasColumnName("pullDetailSn");
            this.Property(t => t.giveAmount).HasColumnName("giveAmount");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
