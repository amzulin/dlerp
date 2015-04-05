using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_StockReportMap : EntityTypeConfiguration<V_StockReport>
    {
        public V_StockReportMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.stockMonth, t.depotId, t.materialNo, t.startAmount, t.startCost, t.inAmount, t.inCost, t.outAmount, t.outCost, t.endAmount, t.endCost, t.createDate, t.depotName, t.materialName, t.xslength });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.stockMonth)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.depotId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.startAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.startCost)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.inAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.inCost)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.outAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.outCost)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.endAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.endCost)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depotName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.category)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.unit2)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_StockReport");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.stockMonth).HasColumnName("stockMonth");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.startAmount).HasColumnName("startAmount");
            this.Property(t => t.startCost).HasColumnName("startCost");
            this.Property(t => t.inAmount).HasColumnName("inAmount");
            this.Property(t => t.inCost).HasColumnName("inCost");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.outCost).HasColumnName("outCost");
            this.Property(t => t.endAmount).HasColumnName("endAmount");
            this.Property(t => t.endCost).HasColumnName("endCost");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.depotName).HasColumnName("depotName");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
        }
    }
}
