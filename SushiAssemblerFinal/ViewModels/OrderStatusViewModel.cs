using System.ComponentModel.DataAnnotations;

namespace SushiAssemblerFinal.ViewModels
{
    public class OrderStatusViewModel
    {
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public string Status { get; set; } = string.Empty;

        public List<string> AvailableStatuses { get; set; } = new()
        {
            "Приета",
            "Подготвя се",
            "Готова за доставка",
            "В доставка",
            "Доставена",
            "Отказана"
        };
    }
}