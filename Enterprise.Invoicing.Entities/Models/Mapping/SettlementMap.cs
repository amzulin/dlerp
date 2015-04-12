using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class SettlementMap : EntityTypeConfiguration<Settlement>
    {
        public SettlementMap()
        {
            // Primary Key
            this.HasKey(t => t.settleNo);

            // Properties
            this.Property(t => t.settleNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Settlement");
            this.Property(t => t.settleNo).HasColumnName("settleNo");
            this.Property(t => t.settleType).HasColumnName("settleType");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.supplierCode).HasColumnName("supplierCode");
            this.Property(t => t.settleStart).HasColumnName("settleStart");
            this.Property(t => t.settleEnd).HasColumnName("settleEnd");
            this.Property(t => t.firstCost).HasColumnName("firstCost");
            this.Property(t => t.returnCost).HasColumnName("returnCost");
            this.Property(t => t.tradeCost).HasColumnName("tradeCost");
            this.Property(t => t.realCost).HasColumnName("realCost");
            this.Property(t => t.badCost).HasColumnName("badCost");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.iscolse).HasColumnName("iscolse");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
