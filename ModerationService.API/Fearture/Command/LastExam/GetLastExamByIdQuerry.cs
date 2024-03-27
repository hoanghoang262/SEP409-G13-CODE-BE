using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class GetLastExamByIdQuery : IRequest<IActionResult>
    {
        public int LastExamId { get; set; }
    }

    public class GetLastExamByIdQueryHandler : IRequestHandler<GetLastExamByIdQuery, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public GetLastExamByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(GetLastExamByIdQuery request, CancellationToken cancellationToken)
        {
            var lastExam = await _context.LastExams.FindAsync(request.LastExamId);

            if (lastExam == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(lastExam);
        }
    }
}
