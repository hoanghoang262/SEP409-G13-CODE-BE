using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class GetLastExamByIdQuery : IRequest<ActionResult<LastExam>>
    {
        public int LastExamId { get; set; }
    }

    public class GetLastExamByIdQueryHandler : IRequestHandler<GetLastExamByIdQuery, ActionResult<LastExam>>
    {
        private readonly Content_ModerationContext _context;

        public GetLastExamByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<LastExam>> Handle(GetLastExamByIdQuery request, CancellationToken cancellationToken)
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
