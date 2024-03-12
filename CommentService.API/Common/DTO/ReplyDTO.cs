namespace CommentService.API.Common.DTO
{
    public class ReplyDTO
    {
        public int Id { get; set; }
        public int? CommentId { get; set; }
        public string? ReplyContent { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Picture { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
