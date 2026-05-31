using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.Extensions;
using SushiAssemblerFinal.Models;
using SushiAssemblerFinal.ViewModels;

namespace SushiAssemblerFinal.Controllers
{
    public class SushiBuilderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public SushiBuilderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await BuildViewModelAsync(new SushiBuilderViewModel());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(SushiBuilderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await BuildViewModelAsync(model);
                return View("Index", model);
            }

            var selectedIngredientIds = new List<int>();

            if (model.RiceId.HasValue)
            {
                selectedIngredientIds.Add(model.RiceId.Value);
            }

            if (model.FishId.HasValue)
            {
                selectedIngredientIds.Add(model.FishId.Value);
            }

            selectedIngredientIds.AddRange(model.SelectedVegetableIds);
            selectedIngredientIds.AddRange(model.SelectedSauceIds);
            selectedIngredientIds.AddRange(model.SelectedExtraIds);

            var ingredients = await _context.Ingredients
                .Where(i => selectedIngredientIds.Contains(i.Id) && i.IsAvailable)
                .ToListAsync();

            var rice = ingredients.FirstOrDefault(i => i.Id == model.RiceId);
            var fish = ingredients.FirstOrDefault(i => i.Id == model.FishId);

            if (rice == null)
            {
                ModelState.AddModelError("RiceId", "Избраният ориз не е валиден.");
            }

            if (fish == null)
            {
                ModelState.AddModelError("FishId", "Избраната риба не е валидна.");
            }

            if (!ModelState.IsValid)
            {
                model = await BuildViewModelAsync(model);
                return View("Index", model);
            }

            decimal basePrice = 3.00m;
            decimal unitPrice = basePrice + ingredients.Sum(i => i.Price);

            var details = string.Join(", ", ingredients.Select(i => i.Name));

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            cart.Add(new CartItem
            {
                Id = $"custom-sushi-{Guid.NewGuid()}",
                ProductId = null,
                ProductName = "Персонализирано суши",
                Details = details,
                Price = unitPrice,
                Quantity = model.Quantity,
                IsCustom = true
            });

            HttpContext.Session.SetObject(CartSessionKey, cart);

            return RedirectToAction("Index", "Cart");
        }

        private async Task<SushiBuilderViewModel> BuildViewModelAsync(SushiBuilderViewModel model)
        {
            var ingredients = await _context.Ingredients
                .Where(i => i.IsAvailable)
                .OrderBy(i => i.Type)
                .ThenBy(i => i.Name)
                .ToListAsync();

            model.RiceOptions = ingredients
                .Where(i => i.Type == "Rice")
                .ToList();

            model.FishOptions = ingredients
                .Where(i => i.Type == "Fish")
                .ToList();

            model.VegetableOptions = ingredients
                .Where(i => i.Type == "Vegetable")
                .ToList();

            model.SauceOptions = ingredients
                .Where(i => i.Type == "Sauce")
                .ToList();

            model.ExtraOptions = ingredients
                .Where(i => i.Type == "Extra")
                .ToList();

            return model;
        }
    }
}