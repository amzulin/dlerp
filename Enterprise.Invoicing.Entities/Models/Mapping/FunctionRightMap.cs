using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class FunctionRightMap : EntityTypeConfiguration<FunctionRight>
    {
        public FunctionRightMap()
        {
            // Primary Key
            this.HasKey(t => t.rightSn);

            // Properties
            this.Property(t => t.functionNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FunctionRight");
            this.Property(t => t.rightSn).HasColumnName("rightSn");
            this.Property(t => t.roleSn).HasColumnName("roleSn");
            this.Property(t => t.functionNo).HasColumnName("functionNo");

            // Relationships
            this.HasOptional(t => t.Function)
                .WithMany(t => t.FunctionRights)
                .HasForeignKey(d => d.functionNo);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.FunctionRights)
                .HasForeignKey(d => d.roleSn);

        }
    }
}
