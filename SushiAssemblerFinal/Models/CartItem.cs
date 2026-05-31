namespace SushiAssemblerFinal.Models
{
    public class CartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int? ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? Details { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsCustom { get; set; }

        public decimal Total => Price * Quantity;
    }
}