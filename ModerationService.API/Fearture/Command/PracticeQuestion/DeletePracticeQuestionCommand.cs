using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.PracticeQuestion
{
    public class DeletePracticeQuestionCommand : IRequest<ActionResult<bool>>
    {
        public int PracticeQuestionId { get; set; }
    }
    public class DeletePracticeQuestionCommandHandler : IRequestHandler<DeletePracticeQuestionCommand, ActionResult<bool>>
    {
        private readonly Content_ModerationContext _context;

        public DeletePracticeQuestionCommandHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<bool>> Handle(DeletePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            var practiceQuestion = await _context.PracticeQuestions.FindAsync(request.PracticeQuestionId);

            if (practiceQuestion == null)
            {
                return new BadRequestObjectResult(Message.MSG31);
            }

            _context.PracticeQuestions.Remove(practiceQuestion);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

