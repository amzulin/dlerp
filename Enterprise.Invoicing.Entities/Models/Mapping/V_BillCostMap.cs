using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class V_BillCostMap : EntityTypeConfiguration<V_BillCost>
    {
        public V_BillCostMap()
        {
            // Primary Key
            this.HasKey(t => new { t.billNo, t.title, t.status, t.valid, t.isover, t.billType, t.createDate, t.makeName });

            // Properties
            this.Property(t => t.billNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.status)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.isover)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.billType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            this.Property(t => t.makeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.checkName)
                .HasMaxLength(50);

            this.Property(t => t.cfoName)
                .HasMaxLength(50);

            this.Property(t => t.bossName)
                .HasMaxLength(50);

            this.Property(t => t.depName)
                .HasMaxLength(50);

            this.Property(t => t.checkMsg)
                .HasMaxLength(200);

            this.Property(t => t.cfoMsg)
                .HasMaxLength(200);

            this.Property(t => t.bossMsg)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("V_BillCost");
            this.Property(t => t.billNo).HasColumnName("billNo");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.staffMake).HasColumnName("staffMake");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.staffCheck).HasColumnName("staffCheck");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.staffCfo).HasColumnName("staffCfo");
            this.Property(t => t.cfoDate).HasColumnName("cfoDate");
            this.Property(t => t.staffBoss).HasColumnName("staffBoss");
            this.Property(t => t.bossDate).HasColumnName("bossDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.billType).HasColumnName("billType");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.checkRes).HasColumnName("checkRes");
            this.Property(t => t.cfoRes).HasColumnName("cfoRes");
            this.Property(t => t.bossRes).HasColumnName("bossRes");
            this.Property(t => t.makeName).HasColumnName("makeName");
            this.Property(t => t.checkName).HasColumnName("checkName");
            this.Property(t => t.cfoName).HasColumnName("cfoName");
            this.Property(t => t.bossName).HasColumnName("bossName");
            this.Property(t => t.depName).HasColumnName("depName");
            this.Property(t => t.checkMsg).HasColumnName("checkMsg");
            this.Property(t => t.cfoMsg).HasColumnName("cfoMsg");
            this.Property(t => t.bossMsg).HasColumnName("bossMsg");
        }
    }
}
