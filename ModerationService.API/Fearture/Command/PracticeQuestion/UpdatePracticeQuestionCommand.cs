using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.PracticeQuestion
{
    public class UpdatePracticeQuestionCommand : IRequest<IActionResult>
    {
        public int PracticeQuestionId { get; set; }
        public PracticeQuestionDTO PracticeQuestion { get; set; }
    }

    public class UpdatePracticeQuestionCommandHandler : IRequestHandler<UpdatePracticeQuestionCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public UpdatePracticeQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<IActionResult> Handle(UpdatePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.PracticeQuestion.CodeForm)
                || string.IsNullOrEmpty(request.PracticeQuestion.Description)
                || string.IsNullOrEmpty(request.PracticeQuestion.TestCaseJava))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            var existingPracticeQuestion = await _context.PracticeQuestions
                .Include(pq => pq.TestCases)
                .FirstOrDefaultAsync(pq => pq.Id == request.PracticeQuestionId);

            // Check if practice question is exist
            if (existingPracticeQuestion == null)
            {
                return new BadRequestObjectResult(Message.MSG31);
            }

            existingPracticeQuestion.CodeForm = request.PracticeQuestion.CodeForm;
            existingPracticeQuestion.Description = request.PracticeQuestion.Description;
            existingPracticeQuestion.TestCaseJava = request.PracticeQuestion.TestCaseJava;
            existingPracticeQuestion.TestCases.Clear();

            foreach (var testCaseDTO in request.PracticeQuestion.TestCases)
            {
                var newTestCase = new TestCase
                {
                    CodeQuestionId = existingPracticeQuestion.Id,
                    InputTypeInt = testCaseDTO.InputTypeInt,
                    InputTypeString = testCaseDTO.InputTypeString,
                    ExpectedResultInt = testCaseDTO.ExpectedResultInt,
                    ExpectedResultString = testCaseDTO.ExpectedResultString,
                    InputTypeBoolean = testCaseDTO.InputTypeBoolean,
                    ExpectedResultBoolean = testCaseDTO.ExpectedResultBoolean,
                    InputTypeArrayInt = testCaseDTO.InputTypeArrayInt,
                    InputTypeArrayString = testCaseDTO.InputTypeArrayString
                };
                existingPracticeQuestion.TestCases.Add(newTestCase);
            }

            await _context.SaveChangesAsync();

            var practiceQuestionDTO = new PracticeQuestionDTO
            {
                Id = existingPracticeQuestion.Id,
                ChapterId = existingPracticeQuestion.ChapterId,
                CodeForm = existingPracticeQuestion.CodeForm,
                Description = existingPracticeQuestion.Description,
                TestCaseJava = existingPracticeQuestion.TestCaseJava,
                TestCases = existingPracticeQuestion.TestCases.Select(tc => new TestCaseDTO
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
