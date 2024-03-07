using System;
using System.Collections.Generic;

namespace CommentService.API.Models
{
    public partial class Comment
    {
        public Comment()
        {
            Replies = new HashSet<Reply>();
        }

        public int Id { get; set; }
        public int? LessonId { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? Date { get; set; }
        public int? CourseId { get; set; }
        public int? ForumPostId { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }
    }
}
