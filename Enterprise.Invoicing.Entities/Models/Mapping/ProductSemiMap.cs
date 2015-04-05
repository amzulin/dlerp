using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductSemiMap : EntityTypeConfiguration<ProductSemi>
    {
        public ProductSemiMap()
        {
            // Primary Key
            this.HasKey(t => t.semiId);

            // Properties
            this.Property(t => t.semiNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.proName)
                .HasMaxLength(100);

            this.Property(t => t.proModel)
                .HasMaxLength(100);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductSemi");
            this.Property(t => t.semiId).HasColumnName("semiId");
            this.Property(t => t.semiNo).HasColumnName("semiNo");
            this.Property(t => t.proName).HasColumnName("proName");
            this.Property(t => t.proModel).HasColumnName("proModel");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.semiDate).HasColumnName("semiDate");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}
