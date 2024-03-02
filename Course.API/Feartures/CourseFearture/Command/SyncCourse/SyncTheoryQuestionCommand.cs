using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncTheoryQuestionCommand : IRequest<IActionResult>, IMapFrom<TheoryQuestionEvent>
    {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public long? Time { get; set; }
        public string? ContentQuestion { get; set; }
        public long? TimeQuestion { get; set; }
        public class asyncQuestionCommandHandler : IRequestHandler<SyncTheoryQuestionCommand, IActionResult>
        {
            private readonly Course_DeployContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public asyncQuestionCommandHandler(Course_DeployContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(SyncTheoryQuestionCommand request, CancellationToken cancellationToken)
            {
                var existingQuestion = await _context.TheoryQuestions.FindAsync(request.Id);
                if (existingQuestion == null)
                {
                    var newQuestion = new TheoryQuestion
                    {
                        Id = request.Id,

                        ContentQuestion = request.ContentQuestion,

                        Time = request.Time,
                        VideoId = request.VideoId,
                        TimeQuestion = request.TimeQuestion

                    };
                    _context.TheoryQuestions.Add(newQuestion);
                    await _context.SaveChangesAsync();
                }
                else
                {

                    existingQuestion.Time = request.Time;
                    existingQuestion.VideoId = request.VideoId;
                    existingQuestion.ContentQuestion = request.ContentQuestion;
                    existingQuestion.Id = request.Id;
                    existingQuestion.TimeQuestion = request.TimeQuestion;
                    await _context.SaveChangesAsync(cancellationToken);

                }

                return new OkObjectResult("done");
            }
        }
    }

}
