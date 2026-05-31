using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SushiAssemblerFinal.Data;
using SushiAssemblerFinal.Extensions;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.IsAvailable);

            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var cartItemId = $"product-{product.Id}";

            var existingItem = cart.FirstOrDefault(x => x.Id == cartItemId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    Id = cartItemId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Details = product.Description,
                    Price = product.Price,
                    Quantity = 1,
                    IsCustom = false
                });
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(string id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(string id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                item.Quantity++;
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);

            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(string id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("Index");
        }
    }
}