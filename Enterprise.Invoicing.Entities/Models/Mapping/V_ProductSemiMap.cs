using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductSemiMap : EntityTypeConfiguration<V_ProductSemi>
    {
        public V_ProductSemiMap()
        {
            // Primary Key
            this.HasKey(t => new { t.semiNo, t.semiDate, t.status });

            // Properties
            this.Property(t => t.semiNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_ProductSemi");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.total).HasColumnName("total");
            this.Property(t => t.semiNo).HasColumnName("semiNo");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.semiDate).HasColumnName("semiDate");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}
