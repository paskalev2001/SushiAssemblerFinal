using System.ComponentModel.DataAnnotations;

namespace SushiAssemblerFinal.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Адресът за доставка е задължителен.")]
        [Display(Name = "Адрес за доставка")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонният номер е задължителен.")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Бележки към поръчката")]
        public string? Notes { get; set; }
    }
}