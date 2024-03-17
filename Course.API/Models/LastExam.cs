using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class LastExam
    {
        public LastExam()
        {
            Exams = new HashSet<Exam>();
        }

        public int Id { get; set; }
        public int? ChapterId { get; set; }
        public int? PercentageCompleted { get; set; }
        public string? Name { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
