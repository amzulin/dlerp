using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomOrderVirtualDetailMap : EntityTypeConfiguration<V_BomOrderVirtualDetail>
    {
        public V_BomOrderVirtualDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.virtualSn, t.detailSn, t.virtualId, t.sAmount, t.sPrice, t.bomId, t.virtualName, t.vAmount, t.vPrice, t.bomOrderNo, t.Amount, t.Price, t.hadBom, t.hadRequire });

            // Properties
            this.Property(t => t.virtualSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.virtualId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(100);

            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.virtualName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.createStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_BomOrderVirtualDetail");
            this.Property(t => t.virtualSn).HasColumnName("virtualSn");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.virtualId).HasColumnName("virtualId");
            this.Property(t => t.sAmount).HasColumnName("sAmount");
            this.Property(t => t.sPrice).HasColumnName("sPrice");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.virtualName).HasColumnName("virtualName");
            this.Property(t => t.vAmount).HasColumnName("vAmount");
            this.Property(t => t.vPrice).HasColumnName("vPrice");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.hadBom).HasColumnName("hadBom");
            this.Property(t => t.hadRequire).HasColumnName("hadRequire");
            this.Property(t => t.createStaff).HasColumnName("createStaff");
        }
    }
}
