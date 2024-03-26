using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class Wishlist
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? UserId { get; set; }
    }
}
