using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class AttachmentMap : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.attrGuid);

            // Properties
            this.Property(t => t.forKey)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.fileGuid)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.fileName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.filePath)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Attachment");
            this.Property(t => t.attrGuid).HasColumnName("attrGuid");
            this.Property(t => t.forKey).HasColumnName("forKey");
            this.Property(t => t.fileGuid).HasColumnName("fileGuid");
            this.Property(t => t.fileName).HasColumnName("fileName");
            this.Property(t => t.filePath).HasColumnName("filePath");
            this.Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}
