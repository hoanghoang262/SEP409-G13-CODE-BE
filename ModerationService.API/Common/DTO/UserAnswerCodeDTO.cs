using CourseService.API.Common.Mapping;
using ModerationService.API.Models;
using System;
using System.Collections.Generic;

namespace ModerationService.API.Common.ModelDTO
{
    public partial class UserAnswerCodeDTO : IMapFrom<UserAnswerCode>
    {
        public int Id { get; set; }
        public int? CodeQuestionId { get; set; }
        public string? AnswerCode { get; set; }
        public int? UserId { get; set; }

       
    }
}
