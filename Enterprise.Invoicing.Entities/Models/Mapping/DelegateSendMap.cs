using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DelegateSendMap : EntityTypeConfiguration<DelegateSend>
    {
        public DelegateSendMap()
        {
            // Primary Key
            this.HasKey(t => t.sendNo);

            // Properties
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
            this.ToTable("DelegateSend");
            this.Property(t => t.sendNo).HasColumnName("sendNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.needDate).HasColumnName("needDate");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.DelegateSends)
                .HasForeignKey(d => d.depId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.DelegateSends)
                .HasForeignKey(d => d.staffId);
            this.HasOptional(t => t.Supplier)
                .WithMany(t => t.DelegateSends)
                .HasForeignKey(d => d.supplierId);

        }
    }
}
