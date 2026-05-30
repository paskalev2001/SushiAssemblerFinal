using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.ViewModels;

namespace SushiAssemblerFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> EditStatus(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderStatusViewModel
            {
                OrderId = order.Id,
                Status = order.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(OrderStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var order = await _context.Orders.FindAsync(model.OrderId);

            if (order == null)
            {
                return NotFound();
            }

            var allowedStatuses = new List<string>
            {
                "Приета",
                "Подготвя се",
                "Готова за доставка",
                "В доставка",
                "Доставена",
                "Отказана"
            };

            if (!allowedStatuses.Contains(model.Status))
            {
                ModelState.AddModelError("Status", "Невалиден статус.");
                return View(model);
            }

            order.Status = model.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = order.Id });
        }
    }
}