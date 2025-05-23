using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prospecto.Models.Infos;

namespace Prospecto.Data.Configuration
{
    public class BranchEntityConfiguration : IEntityTypeConfiguration<BranchInfo>
    {
        public void Configure(EntityTypeBuilder<BranchInfo> builder)
        {
            builder.Property(x => x.SalesGoal).HasColumnType("decimal(18,2)");
        }
    }
}
