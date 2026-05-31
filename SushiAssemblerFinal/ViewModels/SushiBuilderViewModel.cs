using System.ComponentModel.DataAnnotations;
using SushiAssemblerFinal.Models;

namespace SushiAssemblerFinal.ViewModels
{
    public class SushiBuilderViewModel
    {
        [Required(ErrorMessage = "Изберете вид ориз.")]
        [Display(Name = "Ориз")]
        public int? RiceId { get; set; }

        [Required(ErrorMessage = "Изберете риба или основна съставка.")]
        [Display(Name = "Риба / основна съставка")]
        public int? FishId { get; set; }

        [Display(Name = "Зеленчуци")]
        public List<int> SelectedVegetableIds { get; set; } = new();

        [Display(Name = "Сосове")]
        public List<int> SelectedSauceIds { get; set; } = new();

        [Display(Name = "Добавки")]
        public List<int> SelectedExtraIds { get; set; } = new();

        [Range(1, 20, ErrorMessage = "Количество трябва да е между 1 и 20.")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; } = 1;

        public List<Ingredient> RiceOptions { get; set; } = new();

        public List<Ingredient> FishOptions { get; set; } = new();

        public List<Ingredient> VegetableOptions { get; set; } = new();

        public List<Ingredient> SauceOptions { get; set; } = new();

        public List<Ingredient> ExtraOptions { get; set; } = new();
    }
}