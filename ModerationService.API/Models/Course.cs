using System;
using System.Collections.Generic;

namespace ModerationService.API.Models
{
    public partial class Course
    {
        public Course()
        {
            Chapters = new HashSet<Chapter>();
            Moderations = new HashSet<Moderation>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsCompleted { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Moderation> Moderations { get; set; }
    }
}
