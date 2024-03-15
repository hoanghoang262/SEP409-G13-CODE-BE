using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public partial class Merchant
    {
        public Merchant()
        {
            Payments = new HashSet<Payment>();
        }

        public string Id { get; set; } = null!;
        public string? MerchantName { get; set; }
        public string? MerchantWebLink { get; set; }
        public string? MerchantIpnUrl { get; set; }
        public string? MerchantReturnUrl { get; set; }
        public string? SecretKey { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdateBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdateAt { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
