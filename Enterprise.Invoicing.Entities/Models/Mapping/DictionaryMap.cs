using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DictionaryMap : EntityTypeConfiguration<Dictionary>
    {
        public DictionaryMap()
        {
            // Primary Key
            this.HasKey(t => t.dictionaryKey);

            // Properties
            this.Property(t => t.dictionaryKey)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.dictionaryValue)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.dictionaryLable)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Dictionary");
            this.Property(t => t.dictionaryKey).HasColumnName("dictionaryKey");
            this.Property(t => t.dictionaryValue).HasColumnName("dictionaryValue");
            this.Property(t => t.dictionaryLable).HasColumnName("dictionaryLable");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
