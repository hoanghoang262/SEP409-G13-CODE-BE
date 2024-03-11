using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateTheoryQuestionCommand : IRequest<List<int>>
    {
        public int ChapterId { get; set; }
        public List<LessonDTO> TheoryQuestions { get; set; }
    }

    public class CreateTheoryQuestionCommandHandler : IRequestHandler<CreateTheoryQuestionCommand, List<int>>
    {
        private readonly Content_ModerationContext _context;

        public CreateTheoryQuestionCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<List<int>> Handle(CreateTheoryQuestionCommand request, CancellationToken cancellationToken)
        {

            ////var theoryQuestions = await _context.TheoryQuestions.Include(c => c.AnswerOptions).FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

            ////if (theoryQuestions == null)
            ////    return 0;

            ////_context.AnswerOptions.RemoveRange(theoryQuestions.AnswerOptions);

            ////_context.TheoryQuestions.Remove(theoryQuestions);

            ////await _context.SaveChangesAsync();
            //List<int> TheoryId = new List<int>();

            //foreach (var th in request.TheoryQuestions)
            //{

            //    var theoryQuestion = new TheoryQuestion
            //    {
            //        VideoId = th.VideoId,
            //        ContentQuestion = th.ContentQuestion,
            //        Time = th.Time,
            //    };


            //    _context.TheoryQuestions.Add(theoryQuestion);


            //    foreach (var ansOption in th.AnswerOptions)
            //    {
            //        var answerOption = new AnswerOption
            //        {
            //            QuestionId = theoryQuestion.Id, 
            //            OptionsText = ansOption.OptionsText,
            //            CorrectAnswer = ansOption.CorrectAnswer
            //        };

            //        _context.AnswerOptions.Add(answerOption);
            //    }
            //}


            //await _context.SaveChangesAsync();


            //return request.TheoryQuestions.Last().Id;
            return new List<int>();
        }
    }
}
