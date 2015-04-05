using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DepartmentMap : EntityTypeConfiguration<Department>
    {
        public DepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.depId);

            // Properties
            this.Property(t => t.depName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.phone)
                .HasMaxLength(50);

            this.Property(t => t.leader)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Department");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.leader).HasColumnName("leader");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
