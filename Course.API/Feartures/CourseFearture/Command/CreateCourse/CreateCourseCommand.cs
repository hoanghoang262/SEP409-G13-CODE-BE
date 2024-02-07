using CourseService.API.Common.ModelDTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class CreateCourseCommand : IRequest<Course>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

  
        public string Tag { get; set; }
        public int UserId { get; set; }

        public List<ChapterDTO> Chapters { get; set; } 
    }
}
