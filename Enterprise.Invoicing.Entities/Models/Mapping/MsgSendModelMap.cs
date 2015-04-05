using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MsgSendModelMap : EntityTypeConfiguration<MsgSendModel>
    {
        public MsgSendModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.msgId, t.staffId, t.createDate, t.hadAttr, t.staffName, t.title, t.isDelete });

            // Properties
            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.msgcate)
                .HasMaxLength(50);

            this.Property(t => t.receIds)
                .HasMaxLength(500);

            this.Property(t => t.receNames)
                .HasMaxLength(500);

            this.Property(t => t.staffName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.fileGuid)
                .HasMaxLength(100);

            this.Property(t => t.fileName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MsgSendModel");
            this.Property(t => t.msgId).HasColumnName("msgId");
            this.Property(t => t.parentId).HasColumnName("parentId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.msgcate).HasColumnName("msgcate");
            this.Property(t => t.msgcontent).HasColumnName("msgcontent");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.receIds).HasColumnName("receIds");
            this.Property(t => t.receNames).HasColumnName("receNames");
            this.Property(t => t.hadAttr).HasColumnName("hadAttr");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.isDelete).HasColumnName("isDelete");
            this.Property(t => t.deleteDate).HasColumnName("deleteDate");
            this.Property(t => t.fileGuid).HasColumnName("fileGuid");
            this.Property(t => t.fileName).HasColumnName("fileName");
        }
    }
}
