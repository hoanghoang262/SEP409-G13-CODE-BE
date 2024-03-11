using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateAnswerOptionCommand : IRequest<List<int>>
    {
        public List<AnswerOptionsDTO> AnswerOptions { get; set; }
    }

    public class CreateAnswerOptionCommandHandler : IRequestHandler<CreateAnswerOptionCommand, List<int>>
    {
        private readonly Content_ModerationContext _context;

        public CreateAnswerOptionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<List<int>> Handle(CreateAnswerOptionCommand request, CancellationToken cancellationToken)
        {
            var deleteAns = await _context.AnswerOptions.ToListAsync(cancellationToken);
            _context.AnswerOptions.RemoveRange(deleteAns);
            await _context.SaveChangesAsync(cancellationToken);

            List<int> answerOptionsId = new List<int>();
            foreach (var createCommand in request.AnswerOptions)
            {
                var answerOption = new AnswerOption
                {
                    QuestionId = createCommand.QuestionId,
                    OptionsText = createCommand.OptionsText,
                    CorrectAnswer = createCommand.CorrectAnswer
                };

                _context.AnswerOptions.Add(answerOption);
                await _context.SaveChangesAsync();
                answerOptionsId.Add(answerOption.Id);
            }



            return answerOptionsId;
        }
    }

}
