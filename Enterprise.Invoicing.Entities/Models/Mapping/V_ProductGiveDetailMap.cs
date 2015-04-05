using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductGiveDetailMap : EntityTypeConfiguration<V_ProductGiveDetail>
    {
        public V_ProductGiveDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.giveSn, t.giveNo, t.pullDetailSn, t.giveAmount, t.pullNo, t.materialNo, t.theoryAmount, t.realAmount, t.materialName, t.xslength });

            // Properties
            this.Property(t => t.giveSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.giveNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.pullDetailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.giveAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.pullNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.theoryAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.realAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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

            // Table & Column Mappings
            this.ToTable("V_ProductGiveDetail");
            this.Property(t => t.giveSn).HasColumnName("giveSn");
            this.Property(t => t.giveNo).HasColumnName("giveNo");
            this.Property(t => t.pullDetailSn).HasColumnName("pullDetailSn");
            this.Property(t => t.giveAmount).HasColumnName("giveAmount");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.pullNo).HasColumnName("pullNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.theoryAmount).HasColumnName("theoryAmount");
            this.Property(t => t.realAmount).HasColumnName("realAmount");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.price).HasColumnName("price");
        }
    }
}
