using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockReturnDetailMap : EntityTypeConfiguration<StockReturnDetail>
    {
        public StockReturnDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.returnNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.stockoutNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockReturnDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.returnNo).HasColumnName("returnNo");
            this.Property(t => t.stockoutNo).HasColumnName("stockoutNo");
            this.Property(t => t.stockoutDetailSn).HasColumnName("stockoutDetailSn");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.fromDepotId).HasColumnName("fromDepotId");
            this.Property(t => t.toDepotId).HasColumnName("toDepotId");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Depot)
                .WithMany(t => t.StockReturnDetails)
                .HasForeignKey(d => d.toDepotId);
            this.HasRequired(t => t.Depot1)
                .WithMany(t => t.StockReturnDetails1)
                .HasForeignKey(d => d.fromDepotId);
            this.HasRequired(t => t.Material)
                .WithMany(t => t.StockReturnDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasRequired(t => t.StockOut)
                .WithMany(t => t.StockReturnDetails)
                .HasForeignKey(d => d.stockoutNo);
            this.HasRequired(t => t.StockOutDetail)
                .WithMany(t => t.StockReturnDetails)
                .HasForeignKey(d => d.stockoutDetailSn);
            this.HasRequired(t => t.StockReturn)
                .WithMany(t => t.StockReturnDetails)
                .HasForeignKey(d => d.returnNo);

        }
    }
}
