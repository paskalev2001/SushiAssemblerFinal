using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SushiAssemblerFinal.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Адресът за доставка е задължителен.")]
        [StringLength(300, ErrorMessage = "Адресът не може да бъде по-дълъг от 300 символа.")]
        [Display(Name = "Адрес за доставка")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонният номер е задължителен.")]
        [StringLength(30, ErrorMessage = "Телефонният номер е твърде дълъг.")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Бележките не могат да бъдат по-дълги от 500 символа.")]
        [Display(Name = "Бележки")]
        public string? Notes { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Приета";

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}