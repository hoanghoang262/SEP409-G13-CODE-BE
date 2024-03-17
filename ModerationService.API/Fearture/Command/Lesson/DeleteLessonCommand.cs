using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class DeleteLessonCommand : IRequest<ActionResult<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand, ActionResult<int>>
    {
        private readonly Content_ModerationContext _context;

        public DeleteLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<ActionResult<int>> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = await _context.Lessons
                .Include(c => c.TheoryQuestions)
                .ThenInclude(l => l.AnswerOptions)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

            if (lesson == null)
            {
                return new BadRequestObjectResult(Message.MSG29);
            }

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
