using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Feature.Command
{
    public class CreatePracticeQuestionCommand : IRequest<ActionResult<PracticeQuestionDTO>>
    {
        public int ChapterId { get; set; }
        public PracticeQuestionDTO PracticeQuestion { get; set; }
    }

    public class CreatePracticeQuestionCommandHandler : IRequestHandler<CreatePracticeQuestionCommand, ActionResult<PracticeQuestionDTO>>
    {
        private readonly Content_ModerationContext _context;

        public CreatePracticeQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<ActionResult<PracticeQuestionDTO>> Handle(CreatePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.PracticeQuestion.CodeForm)
                || string.IsNullOrEmpty(request.PracticeQuestion.Description)
                || string.IsNullOrEmpty(request.PracticeQuestion.TestCaseJava))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            var chapter = await _context.Chapters
                 .Include(c => c.PracticeQuestions)
                     .ThenInclude(l => l.TestCases)
                 .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

            // Check if chapter is exist
            if (chapter == null)
            {
                return new BadRequestObjectResult(Message.MSG28);
            }

            foreach (var prac in chapter.PracticeQuestions)
            {
                _context.TestCases.RemoveRange(prac.TestCases);
            }

            _context.PracticeQuestions.RemoveRange(chapter.PracticeQuestions);

            await _context.SaveChangesAsync();

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

            return new OkObjectResult(practiceQuestionDTO);
        }
    }
}
