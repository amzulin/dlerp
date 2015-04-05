using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ReturnOutModelMap : EntityTypeConfiguration<V_ReturnOutModel>
    {
        public V_ReturnOutModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.returnNo, t.depId, t.staffId, t.status, t.valid, t.createDate, t.returnType, t.returnName });

            // Properties
            this.Property(t => t.returnNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
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

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            this.Property(t => t.returnType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.returnName)
                .IsRequired()
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("V_ReturnOutModel");
            this.Property(t => t.returnNo).HasColumnName("returnNo");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.returnType).HasColumnName("returnType");
            this.Property(t => t.returnName).HasColumnName("returnName");
        }
    }
}
