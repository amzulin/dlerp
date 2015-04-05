using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_StockChangeModelMap : EntityTypeConfiguration<V_StockChangeModel>
    {
        public V_StockChangeModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.changeNo, t.staffId, t.depId, t.status, t.valid, t.isover, t.createDate });

            // Properties
            this.Property(t => t.changeNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.picker)
                .HasMaxLength(50);

            this.Property(t => t.pickdep)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_StockChangeModel");
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
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
        }
    }
}
