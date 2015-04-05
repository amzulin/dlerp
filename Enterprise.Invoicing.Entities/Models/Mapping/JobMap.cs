using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class JobMap : EntityTypeConfiguration<Job>
    {
        public JobMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Category)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Job");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.OrderInt).HasColumnName("OrderInt");
            this.Property(t => t.Category).HasColumnName("Category");
        }
    }
}
