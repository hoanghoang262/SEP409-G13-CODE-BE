using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.Notes
{
    public class UpdateNoteCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public string ContentNote { get; set; }
        public int? VideoLink { get; set; }


        public class UpdateNoteHandler : IRequestHandler<UpdateNoteCommand, IActionResult>
        {
            private readonly CourseContext _context;

            public UpdateNoteHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
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

                var note = await _context.Notes.FindAsync(request.Id);

                if (note == null)
                {
                    return new  BadRequestObjectResult(Message.MSG39);
                }

                note.LessonId = request.LessonId;
                note.UserId = request.UserId;
                note.ContentNote = request.ContentNote;
                note.VideoLink = request.VideoLink;

                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(Message.MSG16);
            }
        }
    }
}
