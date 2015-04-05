using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockOutDetailMap : EntityTypeConfiguration<StockOutDetail>
    {
        public StockOutDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.stockoutNo)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.changeNo)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockOutDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.stockoutNo).HasColumnName("stockoutNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.orderSn).HasColumnName("orderSn");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.returnAmount).HasColumnName("returnAmount");
            this.Property(t => t.outPrice).HasColumnName("outPrice");
            this.Property(t => t.changeNo).HasColumnName("changeNo");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Depot)
                .WithMany(t => t.StockOutDetails)
                .HasForeignKey(d => d.depotId);
            this.HasRequired(t => t.Material)
                .WithMany(t => t.StockOutDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasOptional(t => t.StockOut)
                .WithMany(t => t.StockOutDetails)
                .HasForeignKey(d => d.stockoutNo);

        }
    }
}
