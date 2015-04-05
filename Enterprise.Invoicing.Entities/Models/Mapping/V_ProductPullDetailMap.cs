using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductPullDetailMap : EntityTypeConfiguration<V_ProductPullDetail>
    {
        public V_ProductPullDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.pullSn, t.pullNo, t.semiId, t.materialNo, t.theoryAmount, t.realAmount, t.materialName, t.xslength, t.productdetailSn, t.productdetailAmount, t.outProductdetailAmount });

            // Properties
            this.Property(t => t.pullSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.pullNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.semiId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.theoryAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.realAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.produceNo)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.unit2)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.productdetailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_ProductPullDetail");
            this.Property(t => t.pullSn).HasColumnName("pullSn");
            this.Property(t => t.pullNo).HasColumnName("pullNo");
            this.Property(t => t.semiId).HasColumnName("semiId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.theoryAmount).HasColumnName("theoryAmount");
            this.Property(t => t.realAmount).HasColumnName("realAmount");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.productdetailSn).HasColumnName("productdetailSn");
            this.Property(t => t.productdetailAmount).HasColumnName("productdetailAmount");
            this.Property(t => t.outProductdetailAmount).HasColumnName("outProductdetailAmount");
        }
    }
}
