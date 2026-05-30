using SushiAssemblerFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace SushiAssemblerFinal.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

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