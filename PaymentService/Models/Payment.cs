using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public partial class Payment
    {
        public Payment()
        {
            PaymentNotifications = new HashSet<PaymentNotification>();
            PaymentSignatures = new HashSet<PaymentSignature>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
        }

        public string PaymentId { get; set; } = null!;
        public string? PaymentContent { get; set; }
        public string? PaymentCurrency { get; set; }
        public string? PaymentRefId { get; set; }
        public decimal? RequriedAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? PaymentLanguage { get; set; }
        public string? MerchantId { get; set; }
        public string? PaymentDestinationId { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentLastMessage { get; set; }
        public int? UserCreateCourseId { get; set; }
        public int? CourseId { get; set; }

        public virtual Merchant? Merchant { get; set; }
        public virtual PaymentDestination? PaymentDestination { get; set; }
        public virtual ICollection<PaymentNotification> PaymentNotifications { get; set; }
        public virtual ICollection<PaymentSignature> PaymentSignatures { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
