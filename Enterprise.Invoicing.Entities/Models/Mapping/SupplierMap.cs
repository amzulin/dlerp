using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class SupplierMap : EntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            // Primary Key
            this.HasKey(t => t.supplierId);

            // Properties
            this.Property(t => t.supplierName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierNo)
                .HasMaxLength(50);

            this.Property(t => t.person)
                .HasMaxLength(50);

            this.Property(t => t.phone)
                .HasMaxLength(50);

            this.Property(t => t.address)
                .HasMaxLength(50);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Supplier");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.supplierNo).HasColumnName("supplierNo");
            this.Property(t => t.person).HasColumnName("person");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.address).HasColumnName("address");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.fax).HasColumnName("fax");
            this.Property(t => t.remark).HasColumnName("remark");
        }
    }
}
