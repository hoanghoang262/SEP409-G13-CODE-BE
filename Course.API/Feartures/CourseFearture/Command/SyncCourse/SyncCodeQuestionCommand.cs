using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.IntegrationEvent.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncCodeQuestionCommand : IRequest<IActionResult>, IMapFrom<CodeQuestionEvent>
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public int? ChapterId { get; set; }
        public class SyncCodeQuestionCommandHandler : IRequestHandler<SyncCodeQuestionCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public SyncCodeQuestionCommandHandler(CourseContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(SyncCodeQuestionCommand request, CancellationToken cancellationToken)
            {
                var existingCodeQuestion = await _context.CodeQuestions.FindAsync(request.Id);
                if (existingCodeQuestion == null)
                {
                    var newCodeQuestion = new CodeQuestion
                    {
                        Id = request.Id,
                        ChapterId= request.ChapterId,
                        Description=request.Description

                    };
                    _context.CodeQuestions.Add(newCodeQuestion);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    existingCodeQuestion.Id = request.Id;
                    existingCodeQuestion.ChapterId = request.ChapterId;
                    existingCodeQuestion.Description = request.Description;
                    await _context.SaveChangesAsync(cancellationToken);

                }




                return new OkObjectResult("done");
            }
        }
    }
}
