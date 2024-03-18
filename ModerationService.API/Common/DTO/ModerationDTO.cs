namespace ModerationService.API.Common.DTO
{
    public class ModerationDTO
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? ChangeType { get; set; }
        public string? ApprovedContent { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public string? CourseName { get; set; }
        public string? CoursePicture { get; set; }
        public string? CourseDescription { get; set; }
        public string? UserName { get; set; }
        public int? PostId { get; set; }
        public string? PostTitle { get; set; }
        public string? Tag { get; set; }
        public string? PostDescription { get; set; }
        public string? PostContent { get; set; }
        public DateTime? PostCreateAt { get; set; }
    }
}
