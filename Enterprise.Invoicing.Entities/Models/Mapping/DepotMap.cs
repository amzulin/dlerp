using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class DepotMap : EntityTypeConfiguration<Depot>
    {
        public DepotMap()
        {
            // Primary Key
            this.HasKey(t => t.depotId);

            // Properties
            this.Property(t => t.depotName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Depot");
            this.Property(t => t.depotId).HasColumnName("depotId");
            this.Property(t => t.depotName).HasColumnName("depotName");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
