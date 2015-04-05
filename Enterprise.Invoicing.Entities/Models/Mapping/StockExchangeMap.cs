using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class StockExchangeMap : EntityTypeConfiguration<StockExchange>
    {
        public StockExchangeMap()
        {
            // Primary Key
            this.HasKey(t => t.changeNo);

            // Properties
            this.Property(t => t.changeNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.picker)
                .HasMaxLength(50);

            this.Property(t => t.pickdep)
                .HasMaxLength(50);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StockExchange");
            this.Property(t => t.changeNo).HasColumnName("changeNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.picker).HasColumnName("picker");
            this.Property(t => t.pickdep).HasColumnName("pickdep");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.StockExchanges)
                .HasForeignKey(d => d.depId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.StockExchanges)
                .HasForeignKey(d => d.staffId);

        }
    }
}
