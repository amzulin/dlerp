using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.menuNo);

            // Properties
            this.Property(t => t.menuNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.parentNo)
                .HasMaxLength(50);

            this.Property(t => t.menuName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.menuUrl)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Menu");
            this.Property(t => t.menuNo).HasColumnName("menuNo");
            this.Property(t => t.parentNo).HasColumnName("parentNo");
            this.Property(t => t.menuName).HasColumnName("menuName");
            this.Property(t => t.menuUrl).HasColumnName("menuUrl");
            this.Property(t => t.menuType).HasColumnName("menuType");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
