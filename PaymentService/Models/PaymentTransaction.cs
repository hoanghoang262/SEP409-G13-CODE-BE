using System;
using System.Collections.Generic;

namespace PaymentService.Models
{
    public partial class PaymentTransaction
    {
        public string Id { get; set; } = null!;
        public string? TranMessage { get; set; }
        public string? TranPayLoad { get; set; }
        public string? TranStatus { get; set; }
        public decimal? TransAmount { get; set; }
        public DateTime? TranDate { get; set; }
        public string? PaymentId { get; set; }
        public int? CourseId { get; set; }
        public int? BuyerId { get; set; }
        public int? UserCreateCourseId { get; set; }
    }
}
