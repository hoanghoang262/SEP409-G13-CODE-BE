namespace PaymentService.Common
{
    public class PaymentDtos
    {
        public string? PaymentId { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CoursePicture { get; set; }
        public Decimal? Money { get; set; }
        public DateTime? TransactionDate { get; set; }   
    }
}
