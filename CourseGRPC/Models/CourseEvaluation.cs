using System;
using System.Collections.Generic;

namespace CourseGRPC.Models
{
    public partial class CourseEvaluation
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CourseId { get; set; }
        public double? Star { get; set; }
    }
}
