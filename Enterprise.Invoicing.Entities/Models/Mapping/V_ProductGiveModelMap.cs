using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductGiveModelMap : EntityTypeConfiguration<V_ProductGiveModel>
    {
        public V_ProductGiveModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.giveNo, t.pullNo, t.giveAmount, t.giveDate, t.status, t.canfs, t.valid, t.isover, t.isclose, t.createDate, t.makeAmount, t.hadgiveAmount, t.materialName, t.xslength });

            // Properties
            this.Property(t => t.giveNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.pullNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.giveAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.produceNo)
                .HasMaxLength(50);

            this.Property(t => t.makeAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.hadgiveAmount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
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

            // Table & Column Mappings
            this.ToTable("V_ProductGiveModel");
            this.Property(t => t.giveNo).HasColumnName("giveNo");
            this.Property(t => t.pullNo).HasColumnName("pullNo");
            this.Property(t => t.giveAmount).HasColumnName("giveAmount");
            this.Property(t => t.giveDate).HasColumnName("giveDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.canfs).HasColumnName("canfs");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.makeAmount).HasColumnName("makeAmount");
            this.Property(t => t.totalorderAmount).HasColumnName("totalorderAmount");
            this.Property(t => t.totalPullAmount).HasColumnName("totalPullAmount");
            this.Property(t => t.totalBackAmount).HasColumnName("totalBackAmount");
            this.Property(t => t.hadgiveAmount).HasColumnName("hadgiveAmount");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.staffId).HasColumnName("staffId");
        }
    }
}
