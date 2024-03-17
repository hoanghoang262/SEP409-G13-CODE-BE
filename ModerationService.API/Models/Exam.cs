using System;
using System.Collections.Generic;

namespace ModerationService.API.Models
{
    public partial class Exam
    {
        public Exam()
        {
            AnswerExams = new HashSet<AnswerExam>();
        }

        public int Id { get; set; }
        public string? ContentQuestion { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public bool? Status { get; set; }
        public int LastExamId { get; set; }

        public virtual LastExam LastExam { get; set; } = null!;
        public virtual ICollection<AnswerExam> AnswerExams { get; set; }
    }
}
