using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class CreateLastExamCommand : IRequest<ActionResult <LastExam>>
    {
        public int ChapterId { get; set; }
        public int? PercentageCompleted { get; set; }
        public string? Name { get; set; }

        public class CreateLastExamCommandHandler : IRequestHandler<CreateLastExamCommand, ActionResult<LastExam>>
        {
            private readonly Content_ModerationContext _context;

            public CreateLastExamCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }

            public async Task<ActionResult<LastExam>> Handle(CreateLastExamCommand request, CancellationToken cancellationToken)
            {
                var lastExam = new LastExam
                {
                    ChapterId = request.ChapterId,
                    PercentageCompleted = request.PercentageCompleted,
                    Name = request.Name
                };

                _context.LastExams.Add(lastExam);
                await _context.SaveChangesAsync();

                return new OkObjectResult(lastExam);
            }
        }
    }
}
