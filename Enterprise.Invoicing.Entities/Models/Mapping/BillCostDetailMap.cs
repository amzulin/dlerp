using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BillCostDetailMap : EntityTypeConfiguration<BillCostDetail>
    {
        public BillCostDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.billNo)
                .HasMaxLength(50);

            this.Property(t => t.billTitle)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BillCostDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.billNo).HasColumnName("billNo");
            this.Property(t => t.billTitle).HasColumnName("billTitle");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.billDate).HasColumnName("billDate");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasOptional(t => t.BillCost)
                .WithMany(t => t.BillCostDetails)
                .HasForeignKey(d => d.billNo);

        }
    }
}
