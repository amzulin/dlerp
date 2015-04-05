using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BomOrderDetailModelMap : EntityTypeConfiguration<V_BomOrderDetailModel>
    {
        public V_BomOrderDetailModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.bomId, t.Amount, t.Price, t.OrderOutAmount, t.bomOrderNo, t.materialName, t.type, t.detailSn, t.status, t.valid, t.createDate, t.hadRequire, t.orderType, t.sellAmount, t.haddelegate, t.hadproduce });

            // Properties
            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.bomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.createStaff)
                .HasMaxLength(50);

            this.Property(t => t.bomSerial)
                .HasMaxLength(50);

            this.Property(t => t.OrderDetailRemark)
                .HasMaxLength(200);

            this.Property(t => t.materialNo)
                .HasMaxLength(50);

            this.Property(t => t.materialCate)
                .HasMaxLength(50);

            this.Property(t => t.bomOrderNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.tunumber)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.detailSn)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.orderType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.supplierName)
                .HasMaxLength(50);

            this.Property(t => t.requireNo)
                .HasMaxLength(50);

            this.Property(t => t.delegateNo)
                .HasMaxLength(50);

            this.Property(t => t.produceNo)
                .HasMaxLength(50);

            this.Property(t => t.version)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_BomOrderDetailModel");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.bomId).HasColumnName("bomId");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.OrderOutAmount).HasColumnName("OrderOutAmount");
            this.Property(t => t.sendDate).HasColumnName("sendDate");
            this.Property(t => t.createStaff).HasColumnName("createStaff");
            this.Property(t => t.bomDate).HasColumnName("bomDate");
            this.Property(t => t.bomSerial).HasColumnName("bomSerial");
            this.Property(t => t.OrderDetailRemark).HasColumnName("OrderDetailRemark");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.parent_Id).HasColumnName("parent_Id");
            this.Property(t => t.materialCate).HasColumnName("materialCate");
            this.Property(t => t.bomOrderNo).HasColumnName("bomOrderNo");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.tunumber).HasColumnName("tunumber");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.detailSn).HasColumnName("detailSn");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.hadRequire).HasColumnName("hadRequire");
            this.Property(t => t.orderType).HasColumnName("orderType");
            this.Property(t => t.supplierName).HasColumnName("supplierName");
            this.Property(t => t.sellAmount).HasColumnName("sellAmount");
            this.Property(t => t.haddelegate).HasColumnName("haddelegate");
            this.Property(t => t.hadproduce).HasColumnName("hadproduce");
            this.Property(t => t.requireNo).HasColumnName("requireNo");
            this.Property(t => t.delegateNo).HasColumnName("delegateNo");
            this.Property(t => t.produceNo).HasColumnName("produceNo");
            this.Property(t => t.version).HasColumnName("version");
        }
    }
}
