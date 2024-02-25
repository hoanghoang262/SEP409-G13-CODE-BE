using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class CodeQuestion
    {
        public CodeQuestion()
        {
            TestCases = new HashSet<TestCase>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<TestCase> TestCases { get; set; }
    }
}
