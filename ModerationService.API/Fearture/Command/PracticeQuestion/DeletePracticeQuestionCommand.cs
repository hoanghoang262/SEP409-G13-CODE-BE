using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.PracticeQuestion
{
    public class DeletePracticeQuestionCommand : IRequest<IActionResult>
    {
        public int PracticeQuestionId { get; set; }
    }
    public class DeletePracticeQuestionCommandHandler : IRequestHandler<DeletePracticeQuestionCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;
        public DeletePracticeQuestionCommandHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(DeletePracticeQuestionCommand request, CancellationToken cancellationToken)
        {
            var practiceQuestion = await _context.PracticeQuestions.Include(c=>c.TestCases).FirstOrDefaultAsync(x=>x.Id.Equals(request.PracticeQuestionId));

            foreach (var test in practiceQuestion.TestCases)
            {
                _context.TestCases.RemoveRange(test);
            }
            if (practiceQuestion == null)
            {
                return new BadRequestObjectResult("Not found");
            }
            _context.PracticeQuestions.Remove(practiceQuestion);
            await _context.SaveChangesAsync();

            return new OkObjectResult("OK");
        }
    }
}

