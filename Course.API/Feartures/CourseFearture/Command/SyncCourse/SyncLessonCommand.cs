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
    public class SyncLessonCommand : IRequest<IActionResult>, IMapFrom<LessonEvent>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public bool? IsCompleted { get; set; }
        public string? ContentLesson { get; set; }
        public string? CodeForm { get; set; }
        public class asyncLessonHandler : IRequestHandler<SyncLessonCommand, IActionResult>
        {
            private readonly CourseContext _context;
          

            public asyncLessonHandler(CourseContext context)
            {
                _context = context;
            
            }
            public async Task<IActionResult> Handle(SyncLessonCommand request, CancellationToken cancellationToken)
            {
                
                var lesson = await _context.Lessons.FindAsync(request.Id);
                if (lesson == null)
                {
                    var newLesson = new Lesson
                    {
                        Id = request.Id,
                        Title = request.Title,
                        VideoUrl = request.VideoUrl,
                        ChapterId = request.ChapterId,
                        Description = request.Description,
                        Duration = request.Duration,
                        ContentLesson=request.ContentLesson,
                        CodeForm=request.CodeForm,
                        
                      

                    };

                    _context.Lessons.Add(newLesson);
                    _context.SaveChanges();

                }
                else
                {

                    lesson.Title = request.Title;
                    lesson.VideoUrl = request.VideoUrl;
                    lesson.ChapterId = request.ChapterId;
                    lesson.Description = request.Description;
                    lesson.Duration = request.Duration;
                    lesson.ContentLesson = request.ContentLesson;
                   

                    await _context.SaveChangesAsync();
                }




                return new OkObjectResult("done");
            }
        }
    }

}
