﻿namespace PaymentService.Common
{
    public class PaymentReturnDTO
    {
        public string? PaymentId { get; set; }
       
        public string? PaymentStatus { get; set; }
        public string? PaymentMessage { get; set; }
       
        public string? PaymentDate { get; set; }
        public string? PaymentRefId { get; set; }
        public decimal? Amount { get; set; }
        public string? Signature { get; set; }
    }
}
