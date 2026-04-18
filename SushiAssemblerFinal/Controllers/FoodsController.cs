using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.Controllers
{
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FoodsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Food.ToListAsync());
        }

        //GET: Foods/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        //POST: Foods/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchFood)
        {
            return View("Index", await _context.Food.Where(j => j.Description.Contains(SearchFood) || j.Name.Contains(SearchFood)).ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Calories,Price")] Food food, IFormFile imageFile)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            bool isAjax = string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase);

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "foods");
                        Directory.CreateDirectory(uploadsRoot);
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsRoot, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        food.ImagePath = "/images/foods/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Image upload failed: " + ex.Message);
                        if (isAjax)
                        {
                            var err = new { success = false, errors = new[] { ex.Message } };
                            return BadRequest(err);
                        }
                        return View(food);
                    }
                }

                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (isAjax)
            {
                var errors = ModelState.Where(kvp => kvp.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage).ToArray()
                    );
                return BadRequest(new { success = false, errors });
            }
            return View(food);
        }

        // GET: Foods/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Calories,Price")] Food food, IFormFile imageFile)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            if (id != food.Id)
            {
                return NotFound();
            }

            bool isAjax = string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase);

            if (ModelState.IsValid)
            {
                var existing = await _context.Food.FindAsync(id);
                if (existing == null) return NotFound();

                // update scalar properties
                existing.Name = food.Name;
                existing.Description = food.Description;
                existing.Calories = food.Calories;
                existing.Price = food.Price;

                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "foods");
                        Directory.CreateDirectory(uploadsRoot);
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsRoot, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // delete old file if exists and is in images/foods
                        if (!string.IsNullOrEmpty(existing.ImagePath))
                        {
                            var oldRelative = existing.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                            var oldFull = Path.Combine(_env.WebRootPath ?? "wwwroot", oldRelative);
                            if (System.IO.File.Exists(oldFull))
                            {
                                System.IO.File.Delete(oldFull);
                            }
                        }

                        existing.ImagePath = "/images/foods/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Image upload failed: " + ex.Message);
                        // return existing model so current image still visible
                        if (isAjax)
                        {
                            return BadRequest(new { success = false, errors = new[] { ex.Message } });
                        }
                        return View(existing);
                    }
                }

                try
                {
                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(existing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // If we got this far, something failed; return ModelState errors for AJAX or the view
            if (isAjax)
            {
                var errors = ModelState.Where(kvp => kvp.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage).ToArray()
                    );
                return BadRequest(new { success = false, errors });
            }
            var original = await _context.Food.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            return View(original ?? food);
        }

        // GET: Foods/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.FindAsync(id);
            if (food != null)
            {
                // delete image file if present
                if (!string.IsNullOrEmpty(food.ImagePath))
                {
                    var oldRelative = food.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                    var oldFull = Path.Combine(_env.WebRootPath ?? "wwwroot", oldRelative);
                    if (System.IO.File.Exists(oldFull)) System.IO.File.Delete(oldFull);
                }

                _context.Food.Remove(food);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.Id == id);
        }
    }
}
