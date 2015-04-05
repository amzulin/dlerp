using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BomOutDetailMap : EntityTypeConfiguration<BomOutDetail>
    {
        public BomOutDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.detailSn);

            // Properties
            this.Property(t => t.bomOutNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BomOutDetail");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.bomOutNo).HasColumnName("bomOutNo");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.amount).HasColumnName("amount");
            this.Property(t => t.remark).HasColumnName("remark");

            // Relationships
            this.HasRequired(t => t.BomMain)
                .WithMany(t => t.BomOutDetails)
                .HasForeignKey(d => d.bomId);
            this.HasRequired(t => t.BomOut)
                .WithMany(t => t.BomOutDetails)
                .HasForeignKey(d => d.bomOutNo);

        }
    }
}
