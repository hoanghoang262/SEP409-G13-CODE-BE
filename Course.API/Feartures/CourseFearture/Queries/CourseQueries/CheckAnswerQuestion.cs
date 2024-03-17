using CourseService.API.Models;
using MediatR; 
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class CheckAnswerQuestion : IRequest<bool>
    {
        public int QuestionId { get; set; }
        public List<int> SelectedOptionIds { get; set; }

        public class CheckAnswerQuestionHandler : IRequestHandler<CheckAnswerQuestion, bool>
        {
            private readonly CourseContext _context;

            public CheckAnswerQuestionHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CheckAnswerQuestion request, CancellationToken cancellationToken)
            {
                var question = await _context.TheoryQuestions
                              .Include(q => q.AnswerOptions)
                               .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

                if (question == null)
                {
                    throw new InvalidOperationException("Question not found.");
                }

                var correctOptionIds = question.AnswerOptions
                    .Where(o => o.CorrectAnswer == true)
                    .Select(o => o.Id);

                return correctOptionIds.All(optionId => request.SelectedOptionIds.Contains(optionId));
            }
        }
    }
}
