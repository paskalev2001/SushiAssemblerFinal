using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SushiAssemblerFinal.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на съставката е задължително.")]
        [StringLength(100, ErrorMessage = "Името не може да бъде по-дълго от 100 символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Типът на съставката е задължителен.")]
        [StringLength(30)]
        [Display(Name = "Тип")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цената е задължителна.")]
        [Range(0, 100, ErrorMessage = "Цената трябва да бъде между 0 и 100 EUR")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Налична")]
        public bool IsAvailable { get; set; } = true;
    }
}