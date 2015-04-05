using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_DelegateBackDetailMap : EntityTypeConfiguration<V_DelegateBackDetail>
    {
        public V_DelegateBackDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.detailSn, t.backNo, t.isProduct, t.materialNo, t.backAmount, t.materialName, t.xslength, t.backDate, t.fromDelegateNo });

            // Properties
            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.backNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.backAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.unit2)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.fromDelegateNo)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_DelegateBackDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.backNo).HasColumnName("backNo");
            this.Property(t => t.isProduct).HasColumnName("isProduct");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.backAmount).HasColumnName("backAmount");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.backDate).HasColumnName("backDate");
            this.Property(t => t.fromDelegateNo).HasColumnName("fromDelegateNo");
        }
    }
}
