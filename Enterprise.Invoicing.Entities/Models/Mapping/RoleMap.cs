using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.roleSn);

            // Properties
            this.Property(t => t.roleName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.roleSn).HasColumnName("roleSn");
            this.Property(t => t.roleName).HasColumnName("roleName");
            this.Property(t => t.showPrice).HasColumnName("showPrice");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
