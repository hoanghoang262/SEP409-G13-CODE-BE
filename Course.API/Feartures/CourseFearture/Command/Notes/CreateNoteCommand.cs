using CourseService.API.Models;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Command.Notes
{
    public class CreateNoteCommand : IRequest<int>
    {
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public string ContentNote { get; set; }
        public int? VideoLink { get; set; }

        public class CreateNoteHandler : IRequestHandler<CreateNoteCommand, int>
        {
            private readonly CourseContext _context;

            public CreateNoteHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
            {
                var note = new Note
                {
                    LessonId = request.LessonId,
                    UserId = request.UserId,
                    ContentNote = request.ContentNote,
                    VideoLink = request.VideoLink
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync(cancellationToken);

                return note.Id;
            }
        }
    }
}
