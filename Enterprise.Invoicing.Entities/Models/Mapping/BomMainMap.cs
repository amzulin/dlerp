using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomMainMap : EntityTypeConfiguration<BomMain>
    {
        public BomMainMap()
        {
            // Primary Key
            this.HasKey(t => t.bomId);

            // Properties
            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.materialCate)
                .HasMaxLength(50);

            this.Property(t => t.otherProject)
                .HasMaxLength(50);

            this.Property(t => t.version)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.bomName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BomMain");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.parent_Id).HasColumnName("parent_Id");
            this.Property(t => t.materialCate).HasColumnName("materialCate");
            this.Property(t => t.otherProject).HasColumnName("otherProject");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.loss).HasColumnName("loss");
            this.Property(t => t.rootCost).HasColumnName("rootCost");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.version).HasColumnName("version");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.isChild).HasColumnName("isChild");
            this.Property(t => t.bomName).HasColumnName("bomName");
            this.Property(t => t.startDate).HasColumnName("startDate");
            this.Property(t => t.endDate).HasColumnName("endDate");
            this.Property(t => t.bomguid).HasColumnName("bomguid");
            this.Property(t => t.rootId).HasColumnName("rootId");

            // Relationships
            this.HasOptional(t => t.Material)
                .WithMany(t => t.BomMains)
                .HasForeignKey(d => d.materialNo);

        }
    }
}
