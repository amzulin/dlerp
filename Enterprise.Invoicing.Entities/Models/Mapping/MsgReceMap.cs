using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class MsgReceMap : EntityTypeConfiguration<MsgRece>
    {
        public MsgReceMap()
        {
            // Primary Key
            this.HasKey(t => t.receId);

            // Properties
            // Table & Column Mappings
            this.ToTable("MsgRece");
            this.Property(t => t.receId).HasColumnName("receId");
            this.Property(t => t.msgId).HasColumnName("msgId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.isRead).HasColumnName("isRead");
            this.Property(t => t.readDate).HasColumnName("readDate");
            this.Property(t => t.isDelete).HasColumnName("isDelete");
            this.Property(t => t.deleteDate).HasColumnName("deleteDate");

            // Relationships
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.MsgReces)
                .HasForeignKey(d => d.staffId);
            this.HasOptional(t => t.MsgSend)
                .WithMany(t => t.MsgReces)
                .HasForeignKey(d => d.msgId);

        }
    }
}
