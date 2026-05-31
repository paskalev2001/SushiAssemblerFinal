using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.ViewModels
{
    public class MenuViewModel
    {
        public List<Product> Products { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public string? Search { get; set; }

        public int? CategoryId { get; set; }
    }
}