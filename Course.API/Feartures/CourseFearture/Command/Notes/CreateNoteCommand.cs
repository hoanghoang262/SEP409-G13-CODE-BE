using Contract.Service.Message;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.Notes
{
    public class CreateNoteCommand : IRequest<IActionResult>
    {
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public string ContentNote { get; set; }
        public int? VideoLink { get; set; }

        public class CreateNoteHandler : IRequestHandler<CreateNoteCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;

            public CreateNoteHandler(CourseContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }

            public async Task<IActionResult> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
            {
                // Validate input
                if (string.IsNullOrEmpty(request.ContentNote))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                // Validate number
                if (request.VideoLink != null && request.VideoLink < 0)
                {
                    return new BadRequestObjectResult(Message.MSG26);
                }
                var user = await _service.SendUserId(request.UserId);

                if (user == null)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }

                var note = new Note
                {
                    LessonId = request.LessonId,
                    UserId = request.UserId,
                    ContentNote = request.ContentNote,
                    VideoLink = request.VideoLink
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(note.Id);
            }
        }
    }
}
