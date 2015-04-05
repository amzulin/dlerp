using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductGiveMap : EntityTypeConfiguration<ProductGive>
    {
        public ProductGiveMap()
        {
            // Primary Key
            this.HasKey(t => t.giveNo);

            // Properties
            this.Property(t => t.giveNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.pullNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductGive");
            this.Property(t => t.giveNo).HasColumnName("giveNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.pullNo).HasColumnName("pullNo");
            this.Property(t => t.giveAmount).HasColumnName("giveAmount");
            this.Property(t => t.giveDate).HasColumnName("giveDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
