using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Feature.Command
{
    public class CreatePracticeQuestionCommand : IRequest<IActionResult>
    {
        public int ChapterId { get; set; }
        public PracticeQuestionDTO PracticeQuestion { get; set; }
    }

    public class CreatePracticeQuestionCommandHandler : IRequestHandler<CreatePracticeQuestionCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public CreatePracticeQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<IActionResult> Handle(CreatePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.PracticeQuestion.CodeForm)
                || string.IsNullOrEmpty(request.PracticeQuestion.Description))
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

            //foreach (var prac in chapter.PracticeQuestions)
            //{
            //    _context.TestCases.RemoveRange(prac.TestCases);
            //}

            //_context.PracticeQuestions.RemoveRange(chapter.PracticeQuestions);

            //await _context.SaveChangesAsync();

            var newPractice = new PracticeQuestion
            {
                ChapterId = request.ChapterId,
                CodeForm = request.PracticeQuestion.CodeForm,
                Description = request.PracticeQuestion.Description,
                TestCaseJava = request.PracticeQuestion.TestCaseJava,

            };

            chapter.PracticeQuestions.Add(newPractice);

           

            await _context.SaveChangesAsync();

            // Convert newPractice to PracticeQuestionDTO and return
            var practiceQuestionDTO = new PracticeQuestionDTO
            {
                Id = newPractice.Id,
                ChapterId = newPractice.ChapterId,
                CodeForm = newPractice.CodeForm,
                Description = newPractice.Description,
                TestCaseJava = newPractice.TestCaseJava,
                TestCaseC=newPractice.TestCaseC,
                TestCaseCplus=newPractice.TestCaseCplus
              
            };

            return new OkObjectResult(practiceQuestionDTO);
        }
    }
}
