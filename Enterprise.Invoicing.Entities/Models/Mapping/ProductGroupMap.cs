using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductGroupMap : EntityTypeConfiguration<ProductGroup>
    {
        public ProductGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.GroupId);

            // Properties
            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            this.Property(t => t.ForAge)
                .HasMaxLength(50);

            this.Property(t => t.EForAge)
                .HasMaxLength(50);

            this.Property(t => t.ForWight)
                .HasMaxLength(50);

            this.Property(t => t.EForWight)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductGroup");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.ForAge).HasColumnName("ForAge");
            this.Property(t => t.EForAge).HasColumnName("EForAge");
            this.Property(t => t.ForWight).HasColumnName("ForWight");
            this.Property(t => t.EForWight).HasColumnName("EForWight");
        }
    }
}
