using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class BillCostMap : EntityTypeConfiguration<BillCost>
    {
        public BillCostMap()
        {
            // Primary Key
            this.HasKey(t => t.billNo);

            // Properties
            this.Property(t => t.billNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.checkMsg)
                .HasMaxLength(200);

            this.Property(t => t.cfoMsg)
                .HasMaxLength(200);

            this.Property(t => t.bossMsg)
                .HasMaxLength(200);

            this.Property(t => t.remark)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BillCost");
            this.Property(t => t.billNo).HasColumnName("billNo");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.staffMake).HasColumnName("staffMake");
            this.Property(t => t.depId).HasColumnName("depId");
            this.Property(t => t.staffCheck).HasColumnName("staffCheck");
            this.Property(t => t.checkRes).HasColumnName("checkRes");
            this.Property(t => t.checkMsg).HasColumnName("checkMsg");
            this.Property(t => t.checkDate).HasColumnName("checkDate");
            this.Property(t => t.staffCfo).HasColumnName("staffCfo");
            this.Property(t => t.cfoMsg).HasColumnName("cfoMsg");
            this.Property(t => t.cfoRes).HasColumnName("cfoRes");
            this.Property(t => t.cfoDate).HasColumnName("cfoDate");
            this.Property(t => t.staffBoss).HasColumnName("staffBoss");
            this.Property(t => t.bossRes).HasColumnName("bossRes");
            this.Property(t => t.bossMsg).HasColumnName("bossMsg");
            this.Property(t => t.bossDate).HasColumnName("bossDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.valid).HasColumnName("valid");
            this.Property(t => t.isover).HasColumnName("isover");
            this.Property(t => t.isclose).HasColumnName("isclose");
            this.Property(t => t.billType).HasColumnName("billType");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.createDate).HasColumnName("createDate");

            // Relationships
            this.HasOptional(t => t.Department)
                .WithMany(t => t.BillCosts)
                .HasForeignKey(d => d.depId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.BillCosts)
                .HasForeignKey(d => d.staffMake);
            this.HasOptional(t => t.Employee1)
                .WithMany(t => t.BillCosts1)
                .HasForeignKey(d => d.staffCheck);
            this.HasOptional(t => t.Employee2)
                .WithMany(t => t.BillCosts2)
                .HasForeignKey(d => d.staffBoss);
            this.HasOptional(t => t.Employee3)
                .WithMany(t => t.BillCosts3)
                .HasForeignKey(d => d.staffCfo);

        }
    }
}
