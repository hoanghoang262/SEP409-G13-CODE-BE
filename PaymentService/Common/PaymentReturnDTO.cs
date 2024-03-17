namespace PaymentService.Common
{
    public class PaymentReturnDTO
    {
        public string? PaymentId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMessage { get; set; }
       
        public string? PaymentDate { get; set; }
        public string? PaymentRefId { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? Signature { get; set; }
        public int? UserCreateCourseId { get; set; }
        public int? CourseId { get; set; }

    }
}
