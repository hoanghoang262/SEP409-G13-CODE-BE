using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Lesson
{
    public class CreateLessonCommand : IRequest<int>
    {
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }
    }

    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public CreateLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = new Models.Lesson
            {
                Title = request.Title,
                VideoUrl = request.VideoUrl,
                ChapterId = request.ChapterId,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                Duration = request.Duration,
                ContentLesson = request.ContentLesson
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return lesson.Id;
        }
    }

}
