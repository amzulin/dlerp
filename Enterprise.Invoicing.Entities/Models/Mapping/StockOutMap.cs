using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockOutMap : EntityTypeConfiguration<StockOut>
    {
        public StockOutMap()
        {
            // Primary Key
            this.HasKey(t => t.stockoutNo);

            // Properties
            this.Property(t => t.stockoutNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.picker)
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.express)
                .HasMaxLength(50);

            this.Property(t => t.expresscode)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockOut");
            this.Property(t => t.stockoutNo).HasColumnName("stockoutNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.bomDetailSn).HasColumnName("bomDetailSn");
            this.Property(t => t.outType).HasColumnName("outType");
            this.Property(t => t.picker).HasColumnName("picker");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.outCost).HasColumnName("outCost");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.express).HasColumnName("express");
            this.Property(t => t.expresscode).HasColumnName("expresscode");
            this.Property(t => t.outDate).HasColumnName("outDate");
            this.Property(t => t.isSettle).HasColumnName("isSettle");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.StockOuts)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.StockOuts)
                .HasForeignKey(d => d.staffId);
            this.HasOptional(t => t.Supplier)
                .WithMany(t => t.StockOuts)
                .HasForeignKey(d => d.supplierId);

        }
    }
}
