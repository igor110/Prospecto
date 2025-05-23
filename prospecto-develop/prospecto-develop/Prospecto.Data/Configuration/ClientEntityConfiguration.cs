using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prospecto.Models.Infos;

namespace Prospecto.Data.Configuration
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientInfo>
    {
        public void Configure(EntityTypeBuilder<ClientInfo> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(x => x.Telephone).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.CPF).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Number).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.Address).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.City).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Complement).HasColumnType("varchar").HasMaxLength(30);
            builder.Property(x => x.Neighborhood).HasColumnType("varchar").HasMaxLength(60);
            builder.Property(x => x.ZipCode).HasColumnType("varchar").HasMaxLength(20);
        }
    }
}
