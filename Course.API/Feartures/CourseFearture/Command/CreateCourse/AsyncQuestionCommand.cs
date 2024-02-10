using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.IntegrationEvent.Event;
using EventBus.Message.IntegrationEvent.PublishEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.PublishEvent;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class AsyncQuestionCommand : IRequest<IActionResult>, IMapFrom<QuestionEvent>
    {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public string? ContentQuestion { get; set; }
        public string? AnswerA { get; set; }
        public string? AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }
        public string? CorrectAnswer { get; set; }
        public long? Time { get; set; }
        public class AsyncQuestionCommandHandler : IRequestHandler<AsyncQuestionCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public AsyncQuestionCommandHandler(CourseContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(AsyncQuestionCommand request, CancellationToken cancellationToken)
            {
                var existingQuestion = await _context.Questions.FindAsync(request.Id);
                if (existingQuestion == null)
                {
                    var newQuestion = new Question
                    {
                        AnswerA = request.AnswerA,
                        AnswerB = request.AnswerB,  
                        AnswerC = request.AnswerC,
                        AnswerD = request.AnswerD,
                        ContentQuestion=request.ContentQuestion,
                        CorrectAnswer=request.CorrectAnswer,
                        Time= request.Time,
                        VideoId=request.VideoId,
                        
                    };

                    _context.Questions.Add(newQuestion);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    existingQuestion.AnswerA = request.AnswerA;
                    existingQuestion.AnswerB = request.AnswerB;
                    existingQuestion.AnswerC = request.AnswerC;
                    existingQuestion.AnswerD = request.AnswerD;
                    existingQuestion.ContentQuestion = request.ContentQuestion;
                    existingQuestion.CorrectAnswer = request.CorrectAnswer;
                    existingQuestion.Time = request.Time;
                    existingQuestion.VideoId = request.VideoId;
                    await _context.SaveChangesAsync(cancellationToken);

                }
              


             
                return new OkObjectResult("done") ;
            }
        }
    }
   
}
