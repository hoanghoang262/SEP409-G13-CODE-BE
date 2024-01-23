using System;
using System.Collections.Generic;

namespace CourseService
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public string? CommentContent { get; set; }
        public string? UserId { get; set; }

        public virtual Lesson? Lesson { get; set; }
    }
}
