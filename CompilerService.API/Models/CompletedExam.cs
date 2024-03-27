using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class CompletedExam
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? LastExamId { get; set; }

        public virtual LastExam? LastExam { get; set; }
    }
}
