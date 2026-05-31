using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SushiAssemblerFinal.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на продукта е задължително.")]
        [StringLength(100, ErrorMessage = "Името не може да бъде по-дълго от 100 символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описанието е задължително.")]
        [StringLength(500, ErrorMessage = "Описанието не може да бъде по-дълго от 500 символа.")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цената е задължителна.")]
        [Range(0.01, 1000, ErrorMessage = "Цената трябва да бъде между 0.01 и 1000 лв.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [StringLength(300, ErrorMessage = "URL адресът към снимката е твърде дълъг.")]
        [Display(Name = "Снимка")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Наличен")]
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Категорията е задължителна.")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}