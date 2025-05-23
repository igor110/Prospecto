using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prospecto.Models.Infos;

namespace Prospecto.Data.Configuration
{
    public class AttendanceEntityConfiguration : IEntityTypeConfiguration<AttendanceInfo>
    {
        public void Configure(EntityTypeBuilder<AttendanceInfo> builder)
        {
            builder.Property(x => x.NameClient).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(x => x.NameProduct).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(x => x.Telephone).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.Value).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ValueClosed).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).HasColumnType("int");

        }
    }
}
