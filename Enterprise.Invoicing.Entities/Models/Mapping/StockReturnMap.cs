using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockReturnMap : EntityTypeConfiguration<StockReturn>
    {
        public StockReturnMap()
        {
            // Primary Key
            this.HasKey(t => t.returnNo);

            // Properties
            this.Property(t => t.returnNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockReturn");
            this.Property(t => t.returnNo).HasColumnName("returnNo");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.returnType).HasColumnName("returnType");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.StockReturns)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.StockReturns)
                .HasForeignKey(d => d.staffId);

        }
    }
}
