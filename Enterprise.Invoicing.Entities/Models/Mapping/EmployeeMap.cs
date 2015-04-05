using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            // Primary Key
            this.HasKey(t => t.staffId);

            // Properties
            this.Property(t => t.staffName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.mobile)
                .HasMaxLength(50);

            this.Property(t => t.email)
                .HasMaxLength(50);

            this.Property(t => t.duty)
                .HasMaxLength(50);

            this.Property(t => t.userId)
                .HasMaxLength(50);

            this.Property(t => t.userPwd)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Employee");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.roleSn).HasColumnName("roleSn");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.mobile).HasColumnName("mobile");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.duty).HasColumnName("duty");
            this.Property(t => t.isUser).HasColumnName("isUser");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.userPwd).HasColumnName("userPwd");
            this.Property(t => t.rigthType).HasColumnName("rigthType");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.depId);
            this.HasOptional(t => t.Role)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.roleSn);

        }
    }
}
