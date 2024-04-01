using System;
using System.Collections.Generic;

namespace CourseGRPC.Models
{
    public partial class UserAnswerCode
    {
        public int Id { get; set; }
        public int? CodeQuestionId { get; set; }
        public string? AnswerCode { get; set; }
        public int? UserId { get; set; }
    }
}
