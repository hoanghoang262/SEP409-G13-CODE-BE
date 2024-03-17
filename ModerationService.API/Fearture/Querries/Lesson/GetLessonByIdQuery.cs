using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Lesson
{
    public class GetLessonByIdQuery : IRequest<LessonDTO>
    {
        public int LessonId { get; set; }
    }
    public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, LessonDTO>
    {
        private readonly Content_ModerationContext _context;

        public GetLessonByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<LessonDTO> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == request.LessonId);

            if (lesson == null)
            {
                return null; 
            }

            var lessonDTO = new LessonDTO
            {
                Id = lesson.Id,
                Title = lesson.Title,
                VideoUrl = lesson.VideoUrl,
                ChapterId = lesson.ChapterId,
                Description = lesson.Description,
                IsCompleted = lesson.IsCompleted,
                Duration = lesson.Duration,
                ContentLesson = lesson.ContentLesson
            };

            return lessonDTO;
        }
    }
}
