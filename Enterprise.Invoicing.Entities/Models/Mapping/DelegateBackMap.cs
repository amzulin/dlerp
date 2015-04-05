using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DelegateBackMap : EntityTypeConfiguration<DelegateBack>
    {
        public DelegateBackMap()
        {
            // Primary Key
            this.HasKey(t => t.backNo);

            // Properties
            this.Property(t => t.backNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.sendNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DelegateBack");
            this.Property(t => t.backNo).HasColumnName("backNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.sendNo).HasColumnName("sendNo");
            this.Property(t => t.backDate).HasColumnName("backDate");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.vaild).HasColumnName("vaild");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
        }
    }
}
