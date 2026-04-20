using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SushiAssemblerFinal.Models
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DeliveryAddress
    {
        public int Id { get; set; }

        // UserId is set server-side; make nullable so model binding does not require it
        [BindNever]
        public string? UserId { get; set; }

        [StringLength(100)]
        public string? RecipientName { get; set; }

        [Phone]
        [StringLength(30)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Street { get; set; } = null!;

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        public bool IsDefault { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
