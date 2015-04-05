using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockReportMap : EntityTypeConfiguration<StockReport>
    {
        public StockReportMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.stockMonth)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockReport");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.stockMonth).HasColumnName("stockMonth");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.startAmount).HasColumnName("startAmount");
            this.Property(t => t.startCost).HasColumnName("startCost");
            this.Property(t => t.inAmount).HasColumnName("inAmount");
            this.Property(t => t.inCost).HasColumnName("inCost");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.outCost).HasColumnName("outCost");
            this.Property(t => t.endAmount).HasColumnName("endAmount");
            this.Property(t => t.endCost).HasColumnName("endCost");
            this.Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}
