using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SushiAssemblerFinal.Models.Food> Food { get; set; } = default!;
    }
}
