namespace ForumService.API.Common.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
       
        public int? LessonId { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? Date { get; set; }
        public int? CourseId { get; set; }
        public int? ForumPostId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string? Picture { get; set;}
    }
}
