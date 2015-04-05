using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class NewsMap : EntityTypeConfiguration<News>
    {
        public NewsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Cate)
                .HasMaxLength(200);

            this.Property(t => t.SubTitle)
                .HasMaxLength(200);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Writer)
                .HasMaxLength(50);

            this.Property(t => t.Info)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("News");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Cate).HasColumnName("Cate");
            this.Property(t => t.SubTitle).HasColumnName("SubTitle");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Writer).HasColumnName("Writer");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
