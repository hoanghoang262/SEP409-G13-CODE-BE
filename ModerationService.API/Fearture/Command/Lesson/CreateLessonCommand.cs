using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Lesson
{
    public class CreateLessonCommand : IRequest<int>
    {
        public int ChapterId { get; set; }
        public LessonDTO TheoryQuestions { get; set; }
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
            var lessons= await _context.Lessons
                .Include(c=>c.Chapter)
                .Include(c=>c.TheoryQuestions)
                .ThenInclude(x=>x.AnswerOptions)
                .FirstOrDefaultAsync(l=>l.Chapter.Id.Equals(request.ChapterId));

            //var lesson = new Models.Lesson
            //{
            //    Title = request.Title,
            //    VideoUrl = request.VideoUrl,
            //    ChapterId = request.ChapterId,
            //    Description = request.Description,
            //    IsCompleted = request.IsCompleted,
            //    Duration = request.Duration,
            //    ContentLesson = request.ContentLesson
            //};

            //_context.Lessons.Add(lesson);
            //await _context.SaveChangesAsync();

            return lessons.Id;
        }
    }

}
