using System;
using System.Collections.Generic;

namespace ModerationService.API.Models
{
    public partial class Moderation
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? ChangeType { get; set; }
        public string? ApprovedContent { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public string? CourseName { get; set; }
        public int? PostId { get; set; }
        public string? PostTitle { get; set; }
    }
}
