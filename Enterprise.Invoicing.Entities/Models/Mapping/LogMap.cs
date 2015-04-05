using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Enterprise.Invoicing.Entities.Models.Mapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            // Primary Key
            this.HasKey(t => t.logSn);

            // Properties
            this.Property(t => t.staffName)
                .HasMaxLength(50);

            this.Property(t => t.logType)
                .HasMaxLength(50);

            this.Property(t => t.logContent)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Logs");
            this.Property(t => t.logSn).HasColumnName("logSn");
            this.Property(t => t.staffName).HasColumnName("staffName");
            this.Property(t => t.logType).HasColumnName("logType");
            this.Property(t => t.logContent).HasColumnName("logContent");
            this.Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}
