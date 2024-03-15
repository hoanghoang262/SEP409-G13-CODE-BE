using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public partial class PaymentDestination
    {
        public PaymentDestination()
        {
            InverseParent = new HashSet<PaymentDestination>();
            Payments = new HashSet<Payment>();
        }

        public string Id { get; set; } = null!;
        public string? DesLogo { get; set; }
        public string? DesShortName { get; set; }
        public string? DesName { get; set; }
        public int? DesShortIndex { get; set; }
        public string? ParentId { get; set; }
        public bool? IsActive { get; set; }

        public virtual PaymentDestination? Parent { get; set; }
        public virtual ICollection<PaymentDestination> InverseParent { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
