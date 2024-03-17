using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.TheoryQuestion
{
    public class GetTheoryQuestionByIdQuery : IRequest<ActionResult<TheoryQuestionDTO>>
    {
        public int TheoryQuestionId { get; set; }
    }
    public class GetTheoryQuestionByIdQueryHandler : IRequestHandler<GetTheoryQuestionByIdQuery, ActionResult<TheoryQuestionDTO>>
    {
        private readonly Content_ModerationContext _context;

        public GetTheoryQuestionByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<TheoryQuestionDTO>> Handle(GetTheoryQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var theoryQuestion = await _context.TheoryQuestions
                .Include(tq => tq.AnswerOptions)
                .FirstOrDefaultAsync(tq => tq.Id == request.TheoryQuestionId);

            // Check if question is exist
            if (theoryQuestion == null)
            {
                return new NotFoundObjectResult(Message.MSG22);
            }

            // Mapping TheoryQuestion entity to TheoryQuestionDTO
            var theoryQuestionDTO = new TheoryQuestionDTO
            {
                Id = theoryQuestion.Id,
                VideoId = theoryQuestion.VideoId,
                TimeQuestion = theoryQuestion.TimeQuestion,
                ContentQuestion = theoryQuestion.ContentQuestion,
                Time = theoryQuestion.Time,
                AnswerOptions = theoryQuestion.AnswerOptions.Select(ao => new AnswerOptionsDTO
                {
                    Id = ao.Id,
                    QuestionId = ao.QuestionId,
                    OptionsText = ao.OptionsText,
                    CorrectAnswer = ao.CorrectAnswer
                }).ToList()
            };

            return new OkObjectResult(theoryQuestionDTO);
        }
    }
}
