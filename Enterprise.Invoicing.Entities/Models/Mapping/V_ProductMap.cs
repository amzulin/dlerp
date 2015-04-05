using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ProductMap : EntityTypeConfiguration<V_Product>
    {
        public V_ProductMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.ProductName, t.OrderInt, t.CreateDate, t.CategoryName, t.CateOrder, t.IndexShow, t.ImgCount, t.ProductEName, t.CategoryEName });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductName)
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

            this.Property(t => t.OrderInt)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CategoryName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CateOrder)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShortInfo)
                .HasMaxLength(2000);

            this.Property(t => t.ForWight)
                .HasMaxLength(50);

            this.Property(t => t.ForAge)
                .HasMaxLength(50);

            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            this.Property(t => t.ImgCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductEName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CategoryEName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EForAge)
                .HasMaxLength(50);

            this.Property(t => t.EForWight)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_Product");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductModel).HasColumnName("ProductModel");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Prokage).HasColumnName("Prokage");
            this.Property(t => t.LWH).HasColumnName("LWH");
            this.Property(t => t.WightOnly).HasColumnName("WightOnly");
            this.Property(t => t.WightAll).HasColumnName("WightAll");
            this.Property(t => t.MoreInfo).HasColumnName("MoreInfo");
            this.Property(t => t.OrderInt).HasColumnName("OrderInt");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");
            this.Property(t => t.CateOrder).HasColumnName("CateOrder");
            this.Property(t => t.ShortInfo).HasColumnName("ShortInfo");
            this.Property(t => t.IndexShow).HasColumnName("IndexShow");
            this.Property(t => t.ForWight).HasColumnName("ForWight");
            this.Property(t => t.ForAge).HasColumnName("ForAge");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.ImgCount).HasColumnName("ImgCount");
            this.Property(t => t.ProductEName).HasColumnName("ProductEName");
            this.Property(t => t.CategoryEName).HasColumnName("CategoryEName");
            this.Property(t => t.EForAge).HasColumnName("EForAge");
            this.Property(t => t.EForWight).HasColumnName("EForWight");
        }
    }
}
