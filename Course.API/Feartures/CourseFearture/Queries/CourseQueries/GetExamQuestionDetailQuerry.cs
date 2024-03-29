using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetExamQuestionDetailQuerry :IRequest<IActionResult>
    {
        public int LastExamId { get; set; }

        public class GetExamQuestionDetailQuerryHandler : IRequestHandler<GetExamQuestionDetailQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public GetExamQuestionDetailQuerryHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(GetExamQuestionDetailQuerry request, CancellationToken cancellationToken)
            {
                var query = await _context.LastExams
                    .Include(c => c.QuestionExams)
                    .ThenInclude(c => c.AnswerExams)
                    .Where(c => c.Id.Equals(request.LastExamId)).ToListAsync();
                if (!query.Any())
                {
                    return new NotFoundObjectResult(Message.MSG22);
                }

                return new OkObjectResult(query);
            }
        }
    }
}
