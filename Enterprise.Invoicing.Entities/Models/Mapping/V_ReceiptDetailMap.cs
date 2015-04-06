using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_ReceiptDetailMap : EntityTypeConfiguration<V_ReceiptDetail>
    {
        public V_ReceiptDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.reprottype, t.showno, t.type, t.status, t.statustype, t.isover, t.createDate, t.material, t.materialNo, t.materialName, t.detail, t.amount1, t.amount2, t.amount3, t.linkfor1, t.linkfor2, t.depot, t.depotid, t.depotname });

            // Properties
            this.Property(t => t.reprottype)
                .IsRequired()
                .HasMaxLength(14);

            this.Property(t => t.staff)
                .HasMaxLength(50);

            this.Property(t => t.deportStaff)
                .HasMaxLength(50);

            this.Property(t => t.dep)
                .HasMaxLength(50);

            this.Property(t => t.checkstaff)
                .HasMaxLength(50);

            this.Property(t => t.showno)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.suuplier)
                .HasMaxLength(50);

            this.Property(t => t.type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.datatype)
                .HasMaxLength(10);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.statustype)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.overtype)
                .HasMaxLength(6);

            this.Property(t => t.remark)
                .HasMaxLength(50);

            this.Property(t => t.material)
                .IsRequired()
                .HasMaxLength(9);

            this.Property(t => t.materialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.bigcate)
                .HasMaxLength(50);

            this.Property(t => t.category)
                .HasMaxLength(50);

            this.Property(t => t.materialModel)
                .HasMaxLength(50);

            this.Property(t => t.materialName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.materialTu)
                .HasMaxLength(50);

            this.Property(t => t.fastcode)
                .HasMaxLength(50);

            this.Property(t => t.pinyin)
                .HasMaxLength(50);

            this.Property(t => t.unit)
                .HasMaxLength(50);

            this.Property(t => t.detail)
                .IsRequired()
                .HasMaxLength(7);

            this.Property(t => t.detailremark)
                .HasMaxLength(50);

            this.Property(t => t.linkno1)
                .HasMaxLength(50);

            this.Property(t => t.linkno2)
                .HasMaxLength(50);

            this.Property(t => t.depot)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.depotid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.depotname)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("V_ReceiptDetail");
            this.Property(t => t.reprottype).HasColumnName("reprottype");
            this.Property(t => t.staffId).HasColumnName("staffId");
            this.Property(t => t.staff).HasColumnName("staff");
            this.Property(t => t.deportStaff).HasColumnName("deportStaff");
            this.Property(t => t.dep).HasColumnName("dep");
            this.Property(t => t.depid).HasColumnName("depid");
            this.Property(t => t.checkstaff).HasColumnName("checkstaff");
            this.Property(t => t.showno).HasColumnName("showno");
            this.Property(t => t.suuplier).HasColumnName("suuplier");
            this.Property(t => t.supplierId).HasColumnName("supplierId");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.datatype).HasColumnName("datatype");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.statustype).HasColumnName("statustype");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.overtype).HasColumnName("overtype");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.material).HasColumnName("material");
            this.Property(t => t.materialNo).HasColumnName("materialNo");
            this.Property(t => t.bigcate).HasColumnName("bigcate");
            this.Property(t => t.category).HasColumnName("category");
            this.Property(t => t.materialModel).HasColumnName("materialModel");
            this.Property(t => t.materialName).HasColumnName("materialName");
            this.Property(t => t.materialTu).HasColumnName("materialTu");
            this.Property(t => t.fastcode).HasColumnName("fastcode");
            this.Property(t => t.pinyin).HasColumnName("pinyin");
            this.Property(t => t.unit).HasColumnName("unit");
            this.Property(t => t.detail).HasColumnName("detail");
            this.Property(t => t.amount1).HasColumnName("amount1");
            this.Property(t => t.amount2).HasColumnName("amount2");
            this.Property(t => t.amount3).HasColumnName("amount3");
            this.Property(t => t.detaildate).HasColumnName("detaildate");
            this.Property(t => t.detailremark).HasColumnName("detailremark");
            this.Property(t => t.linkno1).HasColumnName("linkno1");
            this.Property(t => t.linkfor1).HasColumnName("linkfor1");
            this.Property(t => t.linkno2).HasColumnName("linkno2");
            this.Property(t => t.linkfor2).HasColumnName("linkfor2");
            this.Property(t => t.depot).HasColumnName("depot");
            this.Property(t => t.depotid).HasColumnName("depotid");
            this.Property(t => t.depotname).HasColumnName("depotname");
        }
    }
}
