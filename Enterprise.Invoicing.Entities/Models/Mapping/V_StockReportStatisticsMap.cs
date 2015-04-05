using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_StockReportStatisticsMap : EntityTypeConfiguration<V_StockReportStatistics>
    {
        public V_StockReportStatisticsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.stockMonth, t.depotName, t.depotId });

            // Properties
            this.Property(t => t.stockMonth)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.depotName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.depotId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_StockReportStatistics");
            this.Property(t => t.stockMonth).HasColumnName("stockMonth");
            this.Property(t => t.depotName).HasColumnName("depotName");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.materials).HasColumnName("materials");
            this.Property(t => t.startAmount).HasColumnName("startAmount");
            this.Property(t => t.startCost).HasColumnName("startCost");
            this.Property(t => t.inAmount).HasColumnName("inAmount");
            this.Property(t => t.inCost).HasColumnName("inCost");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.outCost).HasColumnName("outCost");
            this.Property(t => t.endAmount).HasColumnName("endAmount");
            this.Property(t => t.endCost).HasColumnName("endCost");
        }
    }
}
