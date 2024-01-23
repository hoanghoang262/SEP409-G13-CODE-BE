using System;
using System.Collections.Generic;

namespace CourseService
{
    public partial class UserCourseProgress
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CourseId { get; set; }
        public int? CurrentChapterId { get; set; }
        public int? CurrentLessonId { get; set; }
    }
}
