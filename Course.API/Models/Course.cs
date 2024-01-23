using System;
using System.Collections.Generic;

namespace CourseService
{
    public class Course
    {
        public Course()
        {
            Chapters = new HashSet<Chapter>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? UserId { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}
