using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DepotDetailMap : EntityTypeConfiguration<DepotDetail>
    {
        public DepotDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DepotDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.depotAmount).HasColumnName("depotAmount");
            this.Property(t => t.depotCost).HasColumnName("depotCost");
            this.Property(t => t.depotSafe).HasColumnName("depotSafe");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.Depot)
                .WithMany(t => t.DepotDetails)
                .HasForeignKey(d => d.depotId);
            this.HasRequired(t => t.Material)
                .WithMany(t => t.DepotDetails)
                .HasForeignKey(d => d.materialNo);

        }
    }
}
