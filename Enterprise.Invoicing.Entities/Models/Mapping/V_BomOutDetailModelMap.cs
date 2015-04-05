using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomOutDetailModelMap : EntityTypeConfiguration<V_BomOutDetailModel>
    {
        public V_BomOutDetailModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.bomOutNo, t.staffId, t.depId, t.detailSn, t.outAmount, t.status, t.valid, t.createDate, t.bomId, t.amount, t.materialNo, t.bomAmount, t.materialName });

            // Properties
            this.Property(t => t.bomOutNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.staffId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.checkStaff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.detailRemark)
                .HasMaxLength(200);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialCate)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_BomOutDetailModel");
            this.Property(t => t.bomOutNo).HasColumnName("bomOutNo");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.checkStaff).HasColumnName("checkStaff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.detailRemark).HasColumnName("detailRemark");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialCate).HasColumnName("materialCate");
            this.Property(t => t.bomAmount).HasColumnName("bomAmount");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
        }
    }
}
