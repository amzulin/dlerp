using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_NeedPayMap : EntityTypeConfiguration<V_NeedPay>
    {
        public V_NeedPayMap()
        {
            // Primary Key
            this.HasKey(t => new { t.supplierId, t.supplierName });

            // Properties
            this.Property(t => t.supplierId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_NeedPay");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.cost).HasColumnName("cost");
            this.Property(t => t.dateStart).HasColumnName("dateStart");
            this.Property(t => t.dateEnd).HasColumnName("dateEnd");
            this.Property(t => t.amount).HasColumnName("amount");
        }
    }
}
