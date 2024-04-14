using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class CodeUserInLesson
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? LessonId { get; set; }
        public string? UserCode { get; set; }
    }
}
