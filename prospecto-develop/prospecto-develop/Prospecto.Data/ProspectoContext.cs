using Microsoft.EntityFrameworkCore;
using Prospecto.Data.Configuration;
using Prospecto.Models.Enums;
using Prospecto.Models.Infos;

namespace Prospecto.Data
{
    public class ProspectoContext : DbContext
    {
        public ProspectoContext(DbContextOptions<ProspectoContext> options) : base(options) { }

        public DbSet<CompanyInfo> Companies { get; set; }
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<AttendanceInfo> Attendances { get; set; }
        public DbSet<BranchInfo> Branches { get; set; }
        public DbSet<ClientInfo> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BranchEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEntityConfiguration());

            modelBuilder.Entity<UserInfo>().HasData(new UserInfo { Id = 1, Email = "userinfo@email.com", Name = "Adminstrador", TypeUser = UserTypeEnum.ADMINISTRATOR, Password = "*bOj@J!t)Y{*w.W", IsVisible = false, IsActive = true });
        }
    }
}
