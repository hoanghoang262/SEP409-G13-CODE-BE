using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.Event;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncCourseCommand : IRequest<IActionResult>, IMapFrom<CourseEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Price { get; set; }
        public class asyncCourseCommandHandler : IRequestHandler<SyncCourseCommand, IActionResult>
        {
            private readonly CourseContext _context;
            

            public asyncCourseCommandHandler(CourseContext context)
            {
                _context = context;
            
            }
            public async Task<IActionResult> Handle(SyncCourseCommand request, CancellationToken cancellationToken)
            {

               

                var course = await _context.Courses.FindAsync(request.Id);
                if (course == null)
                {
                    var newCourse = new Course
                    {
                        Id = request.Id,
                        Name = request.Name,
                        Description = request.Description,
                        Picture = request.Picture,
                        Tag = request.Tag,
                        CreatedBy = request.CreatedBy,
                        CreatedAt=request.CreatedAt,
                        Price = request.Price,  

                    };

                    _context.Courses.Add(newCourse);
                    await _context.SaveChangesAsync();

                }
                else
                {

                    course.Name = request.Name;
                    course.Description = request.Description;
                    course.Picture = request.Picture;
                    course.Tag = request.Tag;
                    course.CreatedBy = request.CreatedBy;
                    course.Price = request.Price;

                    _context.Courses.Update(course);
                    await _context.SaveChangesAsync();
                }






                return new OkObjectResult("done");
            }
        }
    }

}
