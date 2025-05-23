using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prospecto.Models.Infos;

namespace Prospecto.Data.Configuration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.Property(x => x.Color).HasColumnType("varchar").HasMaxLength(20);
        }
    }
}
