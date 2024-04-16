using PaymentService.API.Models;
using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public partial class PaymentSignature
    {
        public string Id { get; set; } = null!;
        public string? SignValue { get; set; }
        public string? SignAlgo { get; set; }
        public DateTime? SignDate { get; set; }
        public string? SignOwn { get; set; }
        public string? Paymentd { get; set; }

        public virtual Payment? PaymentdNavigation { get; set; }
    }
}
