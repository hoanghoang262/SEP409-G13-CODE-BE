using System;
using System.Collections.Generic;

namespace ModerationService.API.Models
{
    public partial class AnswerOption
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }

        public virtual TheoryQuestion? Question { get; set; }
    }
}
