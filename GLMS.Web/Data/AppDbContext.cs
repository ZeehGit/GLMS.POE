using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial client data
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    ClientId = 1,
                    Name = "Acme Corp",
                    ContactDetails = "acme@email.com",
                    Region = "Africa"
                },
                new Client
                {
                    ClientId = 2,
                    Name = "GlobalTrade Ltd",
                    ContactDetails = "gt@email.com",
                    Region = "Europe"
                }
            );
        }
    }
}