using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomMaterialViewMap : EntityTypeConfiguration<V_BomMaterialView>
    {
        public V_BomMaterialViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.bomId, t.amount, t.loss, t.rootCost, t.materialNo, t.materialName, t.valid, t.xslength, t.status, t.isChild, t.rootId });

            // Properties
            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialCate)
                .HasMaxLength(50);

            this.Property(t => t.loss)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.bomremark)
                .HasMaxLength(200);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.bigcate)
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

            this.Property(t => t.orderNo)
                .HasMaxLength(50);

            this.Property(t => t.xslength)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.materialremark)
                .HasMaxLength(50);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.version)
                .HasMaxLength(50);

            this.Property(t => t.bomName)
                .HasMaxLength(50);

            this.Property(t => t.rootId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("V_BomMaterialView");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.parent_Id).HasColumnName("parent_Id");
            this.Property(t => t.materialCate).HasColumnName("materialCate");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.loss).HasColumnName("loss");
            this.Property(t => t.rootCost).HasColumnName("rootCost");
            this.Property(t => t.bomremark).HasColumnName("bomremark");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.bigcate).HasColumnName("bigcate");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.unit2).HasColumnName("unit2");
            this.Property(t => t.ratio).HasColumnName("ratio");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.orderNo).HasColumnName("orderNo");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.xslength).HasColumnName("xslength");
            this.Property(t => t.materialremark).HasColumnName("materialremark");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.version).HasColumnName("version");
            this.Property(t => t.isChild).HasColumnName("isChild");
            this.Property(t => t.bomName).HasColumnName("bomName");
            this.Property(t => t.startDate).HasColumnName("startDate");
            this.Property(t => t.endDate).HasColumnName("endDate");
            this.Property(t => t.bomguid).HasColumnName("bomguid");
            this.Property(t => t.rootId).HasColumnName("rootId");
        }
    }
}
