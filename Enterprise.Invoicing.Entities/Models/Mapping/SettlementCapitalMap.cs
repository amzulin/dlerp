using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class SettlementCapitalMap : EntityTypeConfiguration<SettlementCapital>
    {
        public SettlementCapitalMap()
        {
            // Primary Key
            this.HasKey(t => t.capitalSn);

            // Properties
            this.Property(t => t.settleNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.createStaff)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SettlementCapital");
            this.Property(t => t.capitalSn).HasColumnName("capitalSn");
            this.Property(t => t.settleNo).HasColumnName("settleNo");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.tradeDate).HasColumnName("tradeDate");
            this.Property(t => t.createStaff).HasColumnName("createStaff");
            this.Property(t => t.tradeCost).HasColumnName("tradeCost");
            this.Property(t => t.badCost).HasColumnName("badCost");
            this.Property(t => t.otherCost).HasColumnName("otherCost");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
