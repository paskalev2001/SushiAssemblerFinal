using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SushiAssemblerFinal.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for delivery addresses
        public ICollection<DeliveryAddress> DeliveryAddresses { get; set; } = new List<DeliveryAddress>();

        // You can add other profile fields here in future (e.g., display name)
    }
}
