using Microsoft.AspNetCore.Mvc;
using SushiAssemblerFinal.Extensions;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private const string CartSessionKey = "Cart";

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var count = cart.Sum(x => x.Quantity);

            return View(count);
        }
    }
}