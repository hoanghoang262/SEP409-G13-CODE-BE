using System;
using System.Collections.Generic;

namespace ModerationService.API.Models
{
    public partial class AnswerExam
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }

        public virtual Exam? Exam { get; set; }
    }
}
