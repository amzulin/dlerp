using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MsgReceModelMap : EntityTypeConfiguration<MsgReceModel>
    {
        public MsgReceModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.msgId, t.createDate, t.hadAttr, t.sendstaffid, t.sendstaffname, t.receId, t.recestaffname, t.isRead, t.isDelete, t.title });

            // Properties
            this.Property(t => t.msgcate)
                .HasMaxLength(50);

            this.Property(t => t.receIds)
                .HasMaxLength(500);

            this.Property(t => t.receNames)
                .HasMaxLength(500);

            this.Property(t => t.sendstaffid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.sendstaffname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.senddepname)
                .HasMaxLength(50);

            this.Property(t => t.receId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.recestaffname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.fileGuid)
                .HasMaxLength(100);

            this.Property(t => t.fileName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MsgReceModel");
            this.Property(t => t.msgId).HasColumnName("msgId");
            this.Property(t => t.parentId).HasColumnName("parentId");
            this.Property(t => t.msgcate).HasColumnName("msgcate");
            this.Property(t => t.msgcontent).HasColumnName("msgcontent");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.receIds).HasColumnName("receIds");
            this.Property(t => t.receNames).HasColumnName("receNames");
            this.Property(t => t.hadAttr).HasColumnName("hadAttr");
            this.Property(t => t.sendstaffid).HasColumnName("sendstaffid");
            this.Property(t => t.sendstaffname).HasColumnName("sendstaffname");
            this.Property(t => t.senddepname).HasColumnName("senddepname");
            this.Property(t => t.receId).HasColumnName("receId");
            this.Property(t => t.recestaffid).HasColumnName("recestaffid");
            this.Property(t => t.recestaffname).HasColumnName("recestaffname");
            this.Property(t => t.isRead).HasColumnName("isRead");
            this.Property(t => t.readDate).HasColumnName("readDate");
            this.Property(t => t.isDelete).HasColumnName("isDelete");
            this.Property(t => t.deleteDate).HasColumnName("deleteDate");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.fileGuid).HasColumnName("fileGuid");
            this.Property(t => t.fileName).HasColumnName("fileName");
        }
    }
}
