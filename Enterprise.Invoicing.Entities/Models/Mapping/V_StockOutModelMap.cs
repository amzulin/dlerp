using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_StockOutModelMap : EntityTypeConfiguration<V_StockOutModel>
    {
        public V_StockOutModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.stockoutNo, t.staffId, t.depId, t.outType, t.outAmount, t.outCost, t.status, t.createDate, t.valid, t.isover });

            // Properties
            this.Property(t => t.stockoutNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.outType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.picker)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.express)
                .HasMaxLength(50);

            this.Property(t => t.expresscode)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_StockOutModel");
            this.Property(t => t.stockoutNo).HasColumnName("stockoutNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.outType).HasColumnName("outType");
            this.Property(t => t.picker).HasColumnName("picker");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.outCost).HasColumnName("outCost");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.bomDetailSn).HasColumnName("bomDetailSn");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.express).HasColumnName("express");
            this.Property(t => t.expresscode).HasColumnName("expresscode");
            this.Property(t => t.outDate).HasColumnName("outDate");
        }
    }
}
