using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreatePracticeQuestionCommand : IRequest<int>
    {
        public int ChapterId { get; set; }
        public PracticeQuestionDTO practiceQuestion { get; set; }
    }

    public class CreatePracticeQuestionCommandHandler : IRequestHandler<CreatePracticeQuestionCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public CreatePracticeQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(CreatePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters
                 .Include(c => c.PracticeQuestions)
                     .ThenInclude(l => l.TestCases)
                 .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

            if (chapter != null)
            {

                foreach (var prac in chapter.PracticeQuestions)
                {
                  
                        _context.TestCases.RemoveRange(prac.TestCases);
                   
                }

                _context.PracticeQuestions.RemoveRange(chapter.PracticeQuestions);


                await _context.SaveChangesAsync();
            }
            var newPractice = new PracticeQuestion
            {
                ChapterId = request.ChapterId,
                CodeForm= request.practiceQuestion.CodeForm,
                Description=request.practiceQuestion.Description,
                TestCaseJava=request.practiceQuestion.TestCaseJava
            };
            chapter.PracticeQuestions.Add(newPractice);
            
            foreach (var test in request.practiceQuestion.TestCases)
            {
                var newTestCase = new TestCase
                {
                    CodeQuestionId=newPractice.Id,
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

            return newPractice.Id;

        }
    }
}
