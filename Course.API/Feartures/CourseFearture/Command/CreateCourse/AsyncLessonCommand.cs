using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.IntegrationEvent.Event;
using EventBus.Message.IntegrationEvent.PublishEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.PublishEvent;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class AsyncLessonCommand : IRequest<IActionResult>, IMapFrom<LessonEvent>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public class AsyncLessonHandler : IRequestHandler<AsyncLessonCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public AsyncLessonHandler(CourseContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(AsyncLessonCommand request, CancellationToken cancellationToken)
            {
                var lesson = await _context.Lessons.FindAsync(request.Id);
                if (lesson == null)
                {
                    var newLesson = new Lesson
                    {
                        Title = request.Title,
                        VideoUrl = request.VideoUrl,
                        ChapterId = request.ChapterId,
                        Description = request.Description,
                        Duration = request.Duration

                    };

                    _context.Lessons.Add(newLesson);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {

                    lesson.Title = request.Title;
                    lesson.VideoUrl = request.VideoUrl;
                    lesson.ChapterId = request.ChapterId;
                    lesson.Description = request.Description;
                    lesson.Duration = request.Duration;
                    await _context.SaveChangesAsync(cancellationToken);
                }
              


             
                return new OkObjectResult("done") ;
            }
        }
    }
   
}
