using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.PracticeQuestion
{
    public class GetPracticeQuestionByIdQuery : IRequest<PracticeQuestionDTO>
    {
        public int PracticeQuestionId { get; set; }
    }
    public class GetPracticeQuestionByIdQueryHandler : IRequestHandler<GetPracticeQuestionByIdQuery, PracticeQuestionDTO>
    {
        private readonly Content_ModerationContext _context;

        public GetPracticeQuestionByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<PracticeQuestionDTO> Handle(GetPracticeQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var practiceQuestion = await _context.PracticeQuestions.Include(pq => pq.TestCases)
                .FirstOrDefaultAsync(pq => pq.Id == request.PracticeQuestionId);

            if (practiceQuestion == null)
            {
                throw new Exception("Practice question not found");
            }

           
            var practiceQuestionDTO = new PracticeQuestionDTO
            {
                Id = practiceQuestion.Id,
                ChapterId = practiceQuestion.ChapterId,
                CodeForm = practiceQuestion.CodeForm,
                Description = practiceQuestion.Description,
                TestCaseJava = practiceQuestion.TestCaseJava,
                TestCases = practiceQuestion.TestCases.Select(tc => new TestCaseDTO
                {
                    Id = tc.Id,
                    CodeQuestionId = tc.CodeQuestionId,
                    InputTypeInt = tc.InputTypeInt,
                    InputTypeString = tc.InputTypeString,
                    ExpectedResultInt = tc.ExpectedResultInt,
                    ExpectedResultString = tc.ExpectedResultString,
                    InputTypeBoolean = tc.InputTypeBoolean,
                    ExpectedResultBoolean = tc.ExpectedResultBoolean,
                    InputTypeArrayInt = tc.InputTypeArrayInt,
                    InputTypeArrayString = tc.InputTypeArrayString
                }).ToList()
            };

            return practiceQuestionDTO;
        }
    }
}
