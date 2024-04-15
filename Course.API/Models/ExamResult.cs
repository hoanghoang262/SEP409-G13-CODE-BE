using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class ExamResult
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? LastExamId { get; set; }
        public decimal? ExamResult1 { get; set; }
    }
}
