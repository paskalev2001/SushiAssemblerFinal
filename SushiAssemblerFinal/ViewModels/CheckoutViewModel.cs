using System.ComponentModel.DataAnnotations;

namespace SushiAssemblerFinal.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Адресът за доставка е задължителен.")]
        [StringLength(300, ErrorMessage = "Адресът не може да бъде по-дълъг от 300 символа.")]
        [Display(Name = "Адрес за доставка")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонният номер е задължителен.")]
        [StringLength(30, ErrorMessage = "Телефонният номер е твърде дълъг.")]
        [RegularExpression(@"^[0-9+\-\s]+$", ErrorMessage = "Телефонът може да съдържа само цифри, интервали, + и -.")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Бележките не могат да бъдат по-дълги от 500 символа.")]
        [Display(Name = "Бележки към поръчката")]
        public string? Notes { get; set; }
    }
}