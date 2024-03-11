using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.TheoryQuestion
{
    public class DeleteTheoryQuestionCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteTheoryQuestionCommandHandler : IRequestHandler<DeleteTheoryQuestionCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public DeleteTheoryQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(DeleteTheoryQuestionCommand request, CancellationToken cancellationToken)
        {
            var theoryQuestion = await _context.TheoryQuestions.Include(c=>c.AnswerOptions).FirstOrDefaultAsync(x=>x.Id.Equals(request.Id));

            if (theoryQuestion == null)
                return 0;

            _context.AnswerOptions.RemoveRange(theoryQuestion.AnswerOptions);

            _context.TheoryQuestions.Remove(theoryQuestion);

            await _context.SaveChangesAsync();

            return theoryQuestion.Id;
        }
    }
}
