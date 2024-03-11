using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class UpdateTheoryQuestionCommand : IRequest<int>
    {
        public int Id { get; set; }
       
        public int? VideoId { get; set; }
        public string? ContentQuestion { get; set; }
        public long? Time { get; set; }
    }

    public class UpdateTheoryQuestionCommandHandler : IRequestHandler<UpdateTheoryQuestionCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public UpdateTheoryQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(UpdateTheoryQuestionCommand request, CancellationToken cancellationToken)
        {
            var theoryQuestion = await _context.TheoryQuestions.FindAsync(request.Id);

            if (theoryQuestion == null)
                return 0;

            
            theoryQuestion.VideoId = request.VideoId;
            theoryQuestion.ContentQuestion = request.ContentQuestion;
            theoryQuestion.Time = request.Time;

            _context.TheoryQuestions.Update(theoryQuestion);
            await _context.SaveChangesAsync();

            return theoryQuestion.Id;
        }
    }
}
