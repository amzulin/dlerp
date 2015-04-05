using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MenuRightMap : EntityTypeConfiguration<MenuRight>
    {
        public MenuRightMap()
        {
            // Primary Key
            this.HasKey(t => t.rightSn);

            // Properties
            this.Property(t => t.menuNo)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MenuRight");
            this.Property(t => t.rightSn).HasColumnName("rightSn");
            this.Property(t => t.roleSn).HasColumnName("roleSn");
            this.Property(t => t.menuNo).HasColumnName("menuNo");

            // Relationships
            this.HasRequired(t => t.Menu)
                .WithMany(t => t.MenuRights)
                .HasForeignKey(d => d.menuNo);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.MenuRights)
                .HasForeignKey(d => d.roleSn);

        }
    }
}
