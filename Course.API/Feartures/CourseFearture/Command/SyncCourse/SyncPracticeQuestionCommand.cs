using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncPracticeQuestionCommand : IRequest<IActionResult>, IMapFrom<PracticeQuestionEvent>
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }
        public class SyncCodeQuestionCommandHandler : IRequestHandler<SyncPracticeQuestionCommand, IActionResult>
        {
            private readonly Course_DeployContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public SyncCodeQuestionCommandHandler(Course_DeployContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(SyncPracticeQuestionCommand request, CancellationToken cancellationToken)
            {
                var existingCodeQuestion = await _context.PracticeQuestions.FindAsync(request.Id);
                if (existingCodeQuestion == null)
                {
                    var newCodeQuestion = new PracticeQuestion
                    {
                        Id = request.Id,
                        ChapterId = request.ChapterId,
                        Description = request.Description,
                        CodeForm = request.CodeForm

                    };
                    _context.PracticeQuestions.Add(newCodeQuestion);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    existingCodeQuestion.Id = request.Id;
                    existingCodeQuestion.ChapterId = request.ChapterId;
                    existingCodeQuestion.Description = request.Description;
                    existingCodeQuestion.CodeForm = request.CodeForm;
                    await _context.SaveChangesAsync(cancellationToken);

                }




                return new OkObjectResult("done");
            }
        }
    }
}
