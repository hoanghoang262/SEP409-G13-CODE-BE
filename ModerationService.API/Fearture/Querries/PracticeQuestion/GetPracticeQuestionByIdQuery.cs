using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.PracticeQuestion
{
    public class GetPracticeQuestionByIdQuery : IRequest<IActionResult>
    {
        public int PracticeQuestionId { get; set; }
    }
    public class GetPracticeQuestionByIdQueryHandler : IRequestHandler<GetPracticeQuestionByIdQuery, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public GetPracticeQuestionByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(GetPracticeQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var practiceQuestion = await _context.PracticeQuestions.Include(pq => pq.TestCases)
                .FirstOrDefaultAsync(pq => pq.Id == request.PracticeQuestionId);

            if (practiceQuestion == null)
            {
                return new BadRequestObjectResult(Message.MSG31);
            }

            var practiceQuestionDTO = new PracticeQuestionDTO
            {
                Id = practiceQuestion.Id,
                ChapterId = practiceQuestion.ChapterId,
                CodeForm = practiceQuestion.CodeForm,
                Description = practiceQuestion.Description,
                TestCaseJava = practiceQuestion.TestCaseJava,
                TestCaseC=practiceQuestion.TestCaseC,
                TestCaseCplus=practiceQuestion.TestCaseCplus,
              
            };

            return new OkObjectResult(practiceQuestionDTO);
        }
    }
}
