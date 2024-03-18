using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class UpdateLastExamCommand : IRequest<ActionResult<LastExam>>
    {
        public int LastExamId { get; set; }
        public int? PercentageCompleted { get; set; }
        public string? Name { get; set; }
    }

    public class UpdateLastExamCommandHandler : IRequestHandler<UpdateLastExamCommand, ActionResult<LastExam>>
    {
        private readonly Content_ModerationContext _context;

        public UpdateLastExamCommandHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<LastExam>> Handle(UpdateLastExamCommand request, CancellationToken cancellationToken)
        {
            var lastExam = await _context.LastExams.FindAsync(request.LastExamId);

            if (lastExam == null)
            {
                return new NotFoundResult();
            }

          
            lastExam.PercentageCompleted = request.PercentageCompleted;
            lastExam.Name = request.Name;

            await _context.SaveChangesAsync();

            return new OkObjectResult(lastExam);
        }
    }
}
