using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.ViewModels;

namespace SushiAssemblerFinal.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsAvailable)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                productsQuery = productsQuery.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            var model = new MenuViewModel
            {
                Products = await productsQuery
                    .OrderBy(p => p.Category!.Name)
                    .ThenBy(p => p.Name)
                    .ToListAsync(),

                Categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync(),

                Search = search,
                CategoryId = categoryId
            };

            return View(model);
        }
    }
}