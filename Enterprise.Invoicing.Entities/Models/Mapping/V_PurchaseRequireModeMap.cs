using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_PurchaseRequireModeMap : EntityTypeConfiguration<V_PurchaseRequireMode>
    {
        public V_PurchaseRequireModeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.requireNo, t.staffId, t.depId, t.status, t.isover, t.createDate, t.valid, t.canfs });

            // Properties
            this.Property(t => t.requireNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_PurchaseRequireMode");
            this.Property(t => t.requireNo).HasColumnName("requireNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.canfs).HasColumnName("canfs");
        }
    }
}
