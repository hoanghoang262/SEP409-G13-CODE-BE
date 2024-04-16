using System;
using System.Collections.Generic;

namespace PaymentService.API.Models
{
    public partial class Payment
    {
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
        public int? BuyerId { get; set; }
    }
}
