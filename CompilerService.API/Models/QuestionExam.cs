using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class QuestionExam
    {
        public QuestionExam()
        {
            AnswerExams = new HashSet<AnswerExam>();
        }

        public int Id { get; set; }
        public string? ContentQuestion { get; set; }
        public int? Score { get; set; }
        public bool? Status { get; set; }
        public int? LastExamId { get; set; }

        public virtual LastExam? LastExam { get; set; }
        public virtual ICollection<AnswerExam> AnswerExams { get; set; }
    }
}
