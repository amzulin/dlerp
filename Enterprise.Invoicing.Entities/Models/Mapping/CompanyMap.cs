using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShowName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.KeyName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.KeyValue)
                .IsRequired();

            this.Property(t => t.Remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Company");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShowName).HasColumnName("ShowName");
            this.Property(t => t.KeyName).HasColumnName("KeyName");
            this.Property(t => t.KeyValue).HasColumnName("KeyValue");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
