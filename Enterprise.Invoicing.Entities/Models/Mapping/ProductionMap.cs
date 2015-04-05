using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductionMap : EntityTypeConfiguration<Production>
    {
        public ProductionMap()
        {
            // Primary Key
            this.HasKey(t => t.produceNo);

            // Properties
            this.Property(t => t.produceNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Production");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.orderDetailSn).HasColumnName("orderDetailSn");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.pullAmount).HasColumnName("pullAmount");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.Productions)
                .HasForeignKey(d => d.depId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.Productions)
                .HasForeignKey(d => d.staffId);
            this.HasOptional(t => t.Material)
                .WithMany(t => t.Productions)
                .HasForeignKey(d => d.materialNo);

        }
    }
}
