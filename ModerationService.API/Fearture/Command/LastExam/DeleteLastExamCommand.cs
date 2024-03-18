using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class DeleteLastExamCommand : IRequest<ActionResult>
    {
        public int LastExamId { get; set; }

        public class DeleteLastExamCommandHandler : IRequestHandler<DeleteLastExamCommand, ActionResult>
        {
            private readonly Content_ModerationContext _context;

            public DeleteLastExamCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(DeleteLastExamCommand request, CancellationToken cancellationToken)
            {
                var lastExam = await _context.LastExams.FindAsync(request.LastExamId);

                if (lastExam == null)
                {
                    return new NotFoundResult();
                }

                _context.LastExams.Remove(lastExam);
                await _context.SaveChangesAsync();

                return new OkResult();
            }
        }
    }
}
