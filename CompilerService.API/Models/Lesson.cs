using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            TheoryQuestions = new HashSet<TheoryQuestion>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }
        public string? CodeForm { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<TheoryQuestion> TheoryQuestions { get; set; }
    }
}
