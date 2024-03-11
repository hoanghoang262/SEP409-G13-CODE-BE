using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.TheoryQuestion
{
    public class CreateTheoryQuestionCommand : IRequest<int>
    {
   
        public int? VideoId { get; set; }
        public string? ContentQuestion { get; set; }
        public long? Time { get; set; }
    }

    public class CreateTheoryQuestionCommandHandler : IRequestHandler<CreateTheoryQuestionCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public CreateTheoryQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(CreateTheoryQuestionCommand request, CancellationToken cancellationToken)
        {
            var theoryQuestion = new Models.TheoryQuestion
            {
                VideoId = request.VideoId,
                ContentQuestion = request.ContentQuestion,
                Time = request.Time
            };

            _context.TheoryQuestions.Add(theoryQuestion);
            await _context.SaveChangesAsync();

            return theoryQuestion.Id;
        }
    }
}
