using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class DeleteLastExamCommand : IRequest<ActionResult>
    {
        public int LastExamId { get; set; }

        public class DeleteLastExamCommandHandler : IRequestHandler<DeleteLastExamCommand, ActionResult>
        {
            private readonly Content_ModerationContext _context;

            public DeleteLastExamCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(DeleteLastExamCommand request, CancellationToken cancellationToken)
            {
                var lastExam = await _context.LastExams
                .Include(c => c.QuestionExams)
                .ThenInclude(l => l.AnswerExams)
                .FirstOrDefaultAsync(x => x.Id.Equals(request.LastExamId));

                if (lastExam == null)
                {
                    return new BadRequestObjectResult(Message.MSG32);
                }

                foreach (var theoryQuestion in lastExam.QuestionExams)
                {
                    _context.AnswerExams.RemoveRange(theoryQuestion.AnswerExams);
                }
                _context.QuestionExams.RemoveRange(lastExam.QuestionExams);

                _context.LastExams.Remove(lastExam);

                await _context.SaveChangesAsync();

                return new OkObjectResult(lastExam.Id);
            }
        }
    }
}
