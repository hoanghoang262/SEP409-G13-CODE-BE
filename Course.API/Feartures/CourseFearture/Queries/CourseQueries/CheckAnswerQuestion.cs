using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class CheckAnswerQuestion : IRequest<IActionResult>
    {
        public int QuestionId { get; set; }
        public List<int> SelectedOptionIds { get; set; }

        public class CheckAnswerQuestionHandler : IRequestHandler<CheckAnswerQuestion, IActionResult>
        {
            private readonly CourseContext _context;

            public CheckAnswerQuestionHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(CheckAnswerQuestion request, CancellationToken cancellationToken)
            {
                var question = await _context.TheoryQuestions
                              .Include(q => q.AnswerOptions)
                               .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

                if (question == null)
                {
                    return new NotFoundObjectResult(Message.MSG31);
                }

                var correctOptionIds = question.AnswerOptions
                    .Where(o => o.CorrectAnswer == true)
                    .Select(o => o.Id);
                var result = correctOptionIds.All(optionId => request.SelectedOptionIds.Contains(optionId));

                return new OkObjectResult(result);
            }
        }
    }
}
