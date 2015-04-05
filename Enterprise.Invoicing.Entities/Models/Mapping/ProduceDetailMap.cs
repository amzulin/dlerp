using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProduceDetailMap : EntityTypeConfiguration<ProduceDetail>
    {
        public ProduceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.produceNo)
                .HasMaxLength(50);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProduceDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.outAmount).HasColumnName("outAmount");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasOptional(t => t.Material)
                .WithMany(t => t.ProduceDetails)
                .HasForeignKey(d => d.materialNo);
            this.HasOptional(t => t.Production)
                .WithMany(t => t.ProduceDetails)
                .HasForeignKey(d => d.produceNo);

        }
    }
}
