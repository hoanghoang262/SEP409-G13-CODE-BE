using System;
using System.Collections.Generic;

namespace CommentService.API.Models
{
    public partial class Reply
    {
        public int Id { get; set; }
        public int? CommentId { get; set; }
        public string? ReplyContent { get; set; }
        public int? UserId { get; set; }

        public virtual Comment? Comment { get; set; }
    }
}
