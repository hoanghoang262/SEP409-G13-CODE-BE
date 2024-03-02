using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            Lessons = new HashSet<Lesson>();
            PracticeQuestions = new HashSet<PracticeQuestion>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<PracticeQuestion> PracticeQuestions { get; set; }
    }
}
