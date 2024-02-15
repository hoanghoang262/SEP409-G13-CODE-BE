using System;
using System.Collections.Generic;

namespace CourseGRPC.Models
{
    public partial class Question
    {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public string? ContentQuestion { get; set; }
        public string? AnswerA { get; set; }
        public string? AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }
        public string? CorrectAnswer { get; set; }
        public long? Time { get; set; }

        public virtual Lesson? Video { get; set; }
    }
}
