using System;
using System.Collections.Generic;

namespace CourseService
{
    public partial class Chapter
    {
        public Chapter()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
