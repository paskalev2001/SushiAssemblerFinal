using SushiAssemblerFinal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SushiAssemblerFinal.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await context.Database.MigrateAsync();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@sushi.com";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, adminPassword);
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            if (await context.Products.AnyAsync())
            {
                return;
            }

            var sushiCategory = new Category { Name = "Суши" };
            var ramenCategory = new Category { Name = "Рамен" };
            var noodlesCategory = new Category { Name = "Нудли" };
            var riceCategory = new Category { Name = "Оризови ястия" };
            var dessertCategory = new Category { Name = "Десерти" };
            var drinksCategory = new Category { Name = "Напитки" };

            context.Categories.AddRange(
                sushiCategory,
                ramenCategory,
                noodlesCategory,
                riceCategory,
                dessertCategory,
                drinksCategory
            );

            context.Products.AddRange(
                new Product
                {
                    Name = "Суши сет Класик",
                    Description = "Комбинация от маки, нигири със сьомга и краставица.",
                    Price = 18.90m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = sushiCategory
                },
                new Product
                {
                    Name = "Сьомга нигири",
                    Description = "Оризова хапка с прясна сьомга.",
                    Price = 7.50m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = sushiCategory
                },
                new Product
                {
                    Name = "Пикантен тон маки",
                    Description = "Маки рулца с пикантен тон и ориз.",
                    Price = 9.90m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = sushiCategory
                },
                new Product
                {
                    Name = "Пилешки рамен",
                    Description = "Японска супа с нудли, пилешко месо, яйце и зеленчуци.",
                    Price = 14.90m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = ramenCategory
                },
                new Product
                {
                    Name = "Мисо рамен",
                    Description = "Рамен с мисо бульон, нудли, яйце и зелен лук.",
                    Price = 13.90m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = ramenCategory
                },
                new Product
                {
                    Name = "Зеленчукови нудли",
                    Description = "Пържени нудли със сезонни зеленчуци и соев сос.",
                    Price = 11.50m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = noodlesCategory
                },
                new Product
                {
                    Name = "Пържен ориз със зеленчуци",
                    Description = "Ориз със зеленчуци, яйце и соев сос.",
                    Price = 10.90m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = riceCategory
                },
                new Product
                {
                    Name = "Мочи",
                    Description = "Японски оризов десерт с пълнеж.",
                    Price = 6.90m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = dessertCategory
                },
                new Product
                {
                    Name = "Зелен чай",
                    Description = "Традиционен японски зелен чай.",
                    Price = 3.50m,
                    ImageUrl = "",
                    IsAvailable = true,
                    Category = drinksCategory
                }
            );

            await context.SaveChangesAsync();
        }
    }
}