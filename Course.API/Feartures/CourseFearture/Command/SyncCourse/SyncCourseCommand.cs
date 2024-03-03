using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.Event;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncCourseCommand : IRequest<IActionResult>, IMapFrom<CourseEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public class asyncCourseCommandHandler : IRequestHandler<SyncCourseCommand, IActionResult>
        {
            private readonly Course_DeployContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public asyncCourseCommandHandler(Course_DeployContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
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

                    };

                    _context.Courses.Add(newCourse);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {

                    course.Name = request.Name;
                    course.Description = request.Description;
                    course.Picture = request.Picture;
                    course.Tag = request.Tag;
                    course.CreatedBy = request.CreatedBy;


                    await _context.SaveChangesAsync(cancellationToken);
                }





                return new OkObjectResult("done");
            }
        }
    }

}
