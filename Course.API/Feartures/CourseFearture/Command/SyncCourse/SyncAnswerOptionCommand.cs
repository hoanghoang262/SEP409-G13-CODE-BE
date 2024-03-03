using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncAnswerOptionCommand : IRequest<IActionResult>, IMapFrom<AnswerOptionsEvent>
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }
        public class SyncAnswerOptionsHandler : IRequestHandler<SyncAnswerOptionCommand, IActionResult>
        {
            private readonly Course_DeployContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public SyncAnswerOptionsHandler(Course_DeployContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(SyncAnswerOptionCommand request, CancellationToken cancellationToken)
            {

                var ans = await _context.AnswerOptions.FindAsync(request.Id);
                if (ans == null)
                {
                    var answerOption = new AnswerOption
                    {
                        Id = request.Id,
                        QuestionId = request.QuestionId,
                        OptionsText = request.OptionsText,
                        CorrectAnswer=request.CorrectAnswer
                    };
                    _context.AnswerOptions.Add(answerOption);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    ans.OptionsText = request.OptionsText;
                    ans.QuestionId = request.QuestionId;
                    await _context.SaveChangesAsync(cancellationToken);
                }


                return new OkObjectResult("done");
            }
        }
    }

}
