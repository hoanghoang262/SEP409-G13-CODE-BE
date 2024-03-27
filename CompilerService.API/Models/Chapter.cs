using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            LastExams = new HashSet<LastExam>();
            Lessons = new HashSet<Lesson>();
            PracticeQuestions = new HashSet<PracticeQuestion>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<LastExam> LastExams { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<PracticeQuestion> PracticeQuestions { get; set; }
    }
}
