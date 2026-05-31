using System.ComponentModel.DataAnnotations;

namespace SushiAssemblerFinal.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на категорията е задължително.")]
        [StringLength(50, ErrorMessage = "Името не може да бъде по-дълго от 50 символа.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}