using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class UpdateLessonCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }
    }

    public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public UpdateLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = await _context.Lessons.FindAsync(request.Id);

            if (lesson == null)
                return 0;

            lesson.Title = request.Title;
            lesson.VideoUrl = request.VideoUrl;
            lesson.ChapterId = request.ChapterId;
            lesson.Description = request.Description;
            lesson.IsCompleted = request.IsCompleted;
            lesson.Duration = request.Duration;
            lesson.ContentLesson = request.ContentLesson;

            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();

            return lesson.Id;
        }
    }

}
