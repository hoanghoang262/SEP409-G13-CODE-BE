using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ModerationService.API.Feature.Command
{
    public class CreatePracticeQuestionCommand : IRequest<PracticeQuestionDTO>
    {
        public int ChapterId { get; set; }
        public PracticeQuestionDTO PracticeQuestion { get; set; }
    }

    public class CreatePracticeQuestionCommandHandler : IRequestHandler<CreatePracticeQuestionCommand, PracticeQuestionDTO>
    {
        private readonly Content_ModerationContext _context;

        public CreatePracticeQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<PracticeQuestionDTO> Handle(CreatePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters
                 .Include(c => c.PracticeQuestions)
                     .ThenInclude(l => l.TestCases)
                 .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

           

            var newPractice = new PracticeQuestion
            {
                ChapterId = request.ChapterId,
                CodeForm = request.PracticeQuestion.CodeForm,
                Description = request.PracticeQuestion.Description,
                TestCaseJava = request.PracticeQuestion.TestCaseJava
            };

            chapter.PracticeQuestions.Add(newPractice);

            foreach (var test in request.PracticeQuestion.TestCases)
            {
                var newTestCase = new TestCase
                {
                    CodeQuestionId = newPractice.Id,
                    InputTypeInt = test.InputTypeInt,
                    InputTypeString = test.InputTypeString,
                    ExpectedResultInt = test.ExpectedResultInt,
                    ExpectedResultString = test.ExpectedResultString,
                    InputTypeBoolean = test.InputTypeBoolean,
                    ExpectedResultBoolean = test.ExpectedResultBoolean,
                    InputTypeArrayInt = test.InputTypeArrayInt,
                    InputTypeArrayString = test.InputTypeArrayString
                };
                newPractice.TestCases.Add(newTestCase);
            }

            await _context.SaveChangesAsync();

            // Convert newPractice to PracticeQuestionDTO and return
            var practiceQuestionDTO = new PracticeQuestionDTO
            {
                Id = newPractice.Id,
                ChapterId = newPractice.ChapterId,
                CodeForm = newPractice.CodeForm,
                Description = newPractice.Description,
                TestCaseJava = newPractice.TestCaseJava,
                TestCases = newPractice.TestCases.Select(tc => new TestCaseDTO
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
