using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class FunctionMap : EntityTypeConfiguration<Function>
    {
        public FunctionMap()
        {
            // Primary Key
            this.HasKey(t => t.functionNo);

            // Properties
            this.Property(t => t.functionNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.parentNo)
                .HasMaxLength(50);

            this.Property(t => t.functionName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Function");
            this.Property(t => t.functionNo).HasColumnName("functionNo");
            this.Property(t => t.parentNo).HasColumnName("parentNo");
            this.Property(t => t.functionName).HasColumnName("functionName");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
