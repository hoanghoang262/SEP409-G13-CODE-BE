using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class DeleteLessonCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public DeleteLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = await _context.Lessons.Include(c=>c.TheoryQuestions).ThenInclude(l=>l.AnswerOptions).FirstOrDefaultAsync(x=>x.Id.Equals(request.Id));

            if (lesson == null)
                return 0;

            foreach (var theoryQuestion in lesson.TheoryQuestions)
            {
                _context.AnswerOptions.RemoveRange(theoryQuestion.AnswerOptions);
            }
            _context.TheoryQuestions.RemoveRange(lesson.TheoryQuestions);

            _context.Lessons.Remove(lesson);

            await _context.SaveChangesAsync();

            return lesson.Id;  
        }
    }
}
