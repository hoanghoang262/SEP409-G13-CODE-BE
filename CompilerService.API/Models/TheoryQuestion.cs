using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class TheoryQuestion
    {
        public TheoryQuestion()
        {
            AnswerOptions = new HashSet<AnswerOption>();
        }

        public int Id { get; set; }
        public int? VideoId { get; set; }
        public long? Time { get; set; }
        public string? ContentQuestion { get; set; }
        public long? TimeQuestion { get; set; }

        public virtual Lesson? Video { get; set; }
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}
