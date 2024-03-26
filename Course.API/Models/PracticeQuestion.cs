using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class PracticeQuestion
    {
        public PracticeQuestion()
        {
            TestCases = new HashSet<TestCase>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }
        public string? TestCaseJava { get; set; }
        public string? TestCaseC { get; set; }
        public string? TestCaseCplus { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<TestCase> TestCases { get; set; }
    }
}
