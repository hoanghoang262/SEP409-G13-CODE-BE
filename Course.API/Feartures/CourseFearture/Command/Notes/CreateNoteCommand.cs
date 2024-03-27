using Contract.Service.Message;
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

            public CreateNoteHandler(CourseContext context)
            {
                _context = context;
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
