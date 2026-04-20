using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SushiAssemblerFinal.Controllers
{
    [Authorize]
    public class DeliveryAddressesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveryAddressesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DeliveryAddresses
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var list = await _context.DeliveryAddresses.Where(d => d.UserId == userId).ToListAsync();
            return View(list);
        }

        // GET: DeliveryAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            var addr = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (addr == null) return NotFound();
            return View(addr);
        }

        // GET: DeliveryAddresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryAddresses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipientName,Phone,Street,City,State,PostalCode,Country,IsDefault")] DeliveryAddress address)
        {
            var userId = _userManager.GetUserId(User);
            bool isAjax = string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase);

            if (!ModelState.IsValid)
            {
                if (isAjax)
                {
                    var errors = ModelState.Where(kvp => kvp.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { success = false, errors });
                }
                return View(address);
            }

            address.UserId = userId;
            if (address.IsDefault)
            {
                var others = _context.DeliveryAddresses.Where(d => d.UserId == userId && d.IsDefault);
                foreach (var o in others) o.IsDefault = false;
            }

            _context.Add(address);
            await _context.SaveChangesAsync();
            if (isAjax) return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });
            return RedirectToAction(nameof(Index));
        }

        // GET: DeliveryAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            var addr = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (addr == null) return NotFound();
            return View(addr);
        }

        // POST: DeliveryAddresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipientName,Phone,Street,City,State,PostalCode,Country,IsDefault")] DeliveryAddress address)
        {
            var userId = _userManager.GetUserId(User);
            bool isAjax = string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase);

            if (id != address.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                if (isAjax)
                {
                    var errors = ModelState.Where(kvp => kvp.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { success = false, errors });
                }
                return View(address);
            }

            var existing = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (existing == null) return NotFound();

            existing.RecipientName = address.RecipientName;
            existing.Phone = address.Phone;
            existing.Street = address.Street;
            existing.City = address.City;
            existing.State = address.State;
            existing.PostalCode = address.PostalCode;
            existing.Country = address.Country;

            if (address.IsDefault)
            {
                var others = _context.DeliveryAddresses.Where(d => d.UserId == userId && d.IsDefault && d.Id != id);
                foreach (var o in others) o.IsDefault = false;
            }
            existing.IsDefault = address.IsDefault;

            try
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DeliveryAddresses.Any(e => e.Id == id && e.UserId == userId)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: DeliveryAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            var addr = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (addr == null) return NotFound();
            return View(addr);
        }

        // POST: DeliveryAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var addr = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (addr != null)
            {
                _context.DeliveryAddresses.Remove(addr);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
