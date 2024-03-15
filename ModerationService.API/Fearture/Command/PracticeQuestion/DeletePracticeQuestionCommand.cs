using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.PracticeQuestion
{
    public class DeletePracticeQuestionCommand : IRequest<bool>
    {
        public int PracticeQuestionId { get; set; }
    }
    public class DeletePracticeQuestionCommandHandler : IRequestHandler<DeletePracticeQuestionCommand, bool>
    {
        private readonly Content_ModerationContext _context;

        public DeletePracticeQuestionCommandHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeletePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            var practiceQuestion = await _context.PracticeQuestions.FindAsync(request.PracticeQuestionId);

            if (practiceQuestion == null)
            {
                throw new Exception("Practice question not found");
            }

            _context.PracticeQuestions.Remove(practiceQuestion);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

