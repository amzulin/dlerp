using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockInMap : EntityTypeConfiguration<StockIn>
    {
        public StockInMap()
        {
            // Primary Key
            this.HasKey(t => t.stockInNo);

            // Properties
            this.Property(t => t.stockInNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockIn");
            this.Property(t => t.stockInNo).HasColumnName("stockInNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.inType).HasColumnName("inType");
            this.Property(t => t.inAmount).HasColumnName("inAmount");
            this.Property(t => t.inCost).HasColumnName("inCost");
            this.Property(t => t.inRemain).HasColumnName("inRemain");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.isSettle).HasColumnName("isSettle");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.StockIns)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.StockIns)
                .HasForeignKey(d => d.staffId);
            this.HasOptional(t => t.Supplier)
                .WithMany(t => t.StockIns)
                .HasForeignKey(d => d.supplierId);

        }
    }
}
