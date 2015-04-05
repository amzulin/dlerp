using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductEName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductModel)
                .HasMaxLength(50);

            this.Property(t => t.Prokage)
                .HasMaxLength(50);

            this.Property(t => t.LWH)
                .HasMaxLength(50);

            this.Property(t => t.WightOnly)
                .HasMaxLength(50);

            this.Property(t => t.WightAll)
                .HasMaxLength(50);

            this.Property(t => t.ShortInfo)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Product");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductEName).HasColumnName("ProductEName");
            this.Property(t => t.ProductModel).HasColumnName("ProductModel");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Prokage).HasColumnName("Prokage");
            this.Property(t => t.LWH).HasColumnName("LWH");
            this.Property(t => t.WightOnly).HasColumnName("WightOnly");
            this.Property(t => t.WightAll).HasColumnName("WightAll");
            this.Property(t => t.ShortInfo).HasColumnName("ShortInfo");
            this.Property(t => t.MoreInfo).HasColumnName("MoreInfo");
            this.Property(t => t.IndexShow).HasColumnName("IndexShow");
            this.Property(t => t.OrderInt).HasColumnName("OrderInt");
            this.Property(t => t.ImgCount).HasColumnName("ImgCount");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships
            this.HasOptional(t => t.ProductCategory)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.CategoryId);
            this.HasOptional(t => t.ProductGroup)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.GroupId);

        }
    }
}
