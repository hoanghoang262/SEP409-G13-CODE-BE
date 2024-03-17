using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class DeleteChapterCommand : IRequest<ActionResult<int>>
    {
        public int ChapterId { get; set; }
    }

    public class DeleteChapterCommandHandler : IRequestHandler<DeleteChapterCommand, ActionResult<int>>
    {
        private readonly Content_ModerationContext _context;

        public DeleteChapterCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<ActionResult<int>> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters
                 .Include(c => c.Lessons)
                     .ThenInclude(l => l.TheoryQuestions)
                         .ThenInclude(tq => tq.AnswerOptions)
                 .Include(c => c.PracticeQuestions)
                     .ThenInclude(pq => pq.TestCases)
                 .Include(c => c.PracticeQuestions)
                     .ThenInclude(pq => pq.UserAnswerCodes)
                 .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

            if (chapter == null)
            {
                return new BadRequestObjectResult(Message.MSG28);
            }

            foreach (var practiceQuestion in chapter.PracticeQuestions)
            {
                _context.TestCases.RemoveRange(practiceQuestion.TestCases);
                _context.UserAnswerCodes.RemoveRange(practiceQuestion.UserAnswerCodes);
            }

            _context.PracticeQuestions.RemoveRange(chapter.PracticeQuestions);

            foreach (var lesson in chapter.Lessons)
            {
                foreach (var theoryQuestion in lesson.TheoryQuestions)
                {
                    _context.AnswerOptions.RemoveRange(theoryQuestion.AnswerOptions);
                }
                _context.TheoryQuestions.RemoveRange(lesson.TheoryQuestions);
            }

            _context.Lessons.RemoveRange(chapter.Lessons);
            _context.Chapters.Remove(chapter);

            await _context.SaveChangesAsync();

            return new OkObjectResult(chapter.Id);
        }
    }
}
