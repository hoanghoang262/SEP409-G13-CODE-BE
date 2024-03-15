using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public partial class PaymentNotification
    {
        public string Id { get; set; } = null!;
        public string? PaymentRefId { get; set; }
        public string? NotiDate { get; set; }
        public string? NotiAmount { get; set; }
        public string? NotiContent { get; set; }
        public string? NotiMessage { get; set; }
        public string? NotiSignature { get; set; }
        public string? PaymentId { get; set; }
        public string? MerchantId { get; set; }
        public string? NotiStatus { get; set; }
        public string? NotiResDate { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
