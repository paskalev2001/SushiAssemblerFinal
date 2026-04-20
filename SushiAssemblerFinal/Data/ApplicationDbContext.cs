using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SushiAssemblerFinal.Models.Food> Food { get; set; } = default!;
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; } = default!;
    }
}
