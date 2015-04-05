using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MsgSendMap : EntityTypeConfiguration<MsgSend>
    {
        public MsgSendMap()
        {
            // Primary Key
            this.HasKey(t => t.msgId);

            // Properties
            this.Property(t => t.msgcate)
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.receIds)
                .HasMaxLength(500);

            this.Property(t => t.receNames)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("MsgSend");
            this.Property(t => t.msgId).HasColumnName("msgId");
            this.Property(t => t.parentId).HasColumnName("parentId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.msgcate).HasColumnName("msgcate");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.msgcontent).HasColumnName("msgcontent");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.receIds).HasColumnName("receIds");
            this.Property(t => t.receNames).HasColumnName("receNames");
            this.Property(t => t.hadAttr).HasColumnName("hadAttr");
            this.Property(t => t.isDelete).HasColumnName("isDelete");
            this.Property(t => t.deleteDate).HasColumnName("deleteDate");

            // Relationships
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.MsgSends)
                .HasForeignKey(d => d.staffId);

        }
    }
}
