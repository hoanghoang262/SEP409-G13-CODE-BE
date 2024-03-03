using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class PracticeQuestion
    {
        public PracticeQuestion()
        {
            TestCases = new HashSet<TestCase>();
            UserAnswerCodes = new HashSet<UserAnswerCode>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<TestCase> TestCases { get; set; }
        public virtual ICollection<UserAnswerCode> UserAnswerCodes { get; set; }
    }
}
