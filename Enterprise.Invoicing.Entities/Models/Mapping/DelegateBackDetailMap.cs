using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DelegateBackDetailMap : EntityTypeConfiguration<DelegateBackDetail>
    {
        public DelegateBackDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.backNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.fromDelegateNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("DelegateBackDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.backNo).HasColumnName("backNo");
            this.Property(t => t.fromDelegateNo).HasColumnName("fromDelegateNo");
            this.Property(t => t.isProduct).HasColumnName("isProduct");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
