using System.ComponentModel.DataAnnotations.Schema;

namespace SushiAssemblerFinal.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public int? ProductId { get; set; }

        public Product? Product { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ItemDetails { get; set; }

        public bool IsCustom { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public decimal Total => UnitPrice * Quantity;
    }
}