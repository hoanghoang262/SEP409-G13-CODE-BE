using System;
using System.Collections.Generic;

namespace ModerationService.API.Models
{
    public partial class CorrectAnswer
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? OptionsId { get; set; }

        public virtual TheoryQuestion? Question { get; set; }
    }
}
