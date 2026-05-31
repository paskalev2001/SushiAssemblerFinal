using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminIngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminIngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ingredients = await _context.Ingredients
                .OrderBy(i => i.Type)
                .ThenBy(i => i.Name)
                .ToListAsync();

            return View(ingredients);
        }

        public IActionResult Create()
        {
            return View(new Ingredient
            {
                IsAvailable = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Ingredients.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await IngredientExists(ingredient.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IngredientExists(int id)
        {
            return await _context.Ingredients.AnyAsync(i => i.Id == id);
        }
    }
}