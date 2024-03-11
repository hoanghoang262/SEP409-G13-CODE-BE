using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.AnswerOptions
{
    public class DeleteAnswerOptionCommand : IRequest<IActionResult>
    {
        public List<int> id { get; set; }
    }

    public class DeleteAnswerOptionCommandHandler : IRequestHandler<DeleteAnswerOptionCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public DeleteAnswerOptionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<IActionResult> Handle(DeleteAnswerOptionCommand request, CancellationToken cancellationToken)
        {

            foreach (var id in request.id)
            {
                var answerOption = await _context.AnswerOptions.FindAsync(id);

                if (answerOption == null)
                    return new BadRequestObjectResult("Not Found");

                _context.AnswerOptions.Remove(answerOption);
                await _context.SaveChangesAsync();
            }

            return new OkObjectResult("Delete Success");
        }
    }
}
