using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.Notes
{
    public class DeleteNoteCommand : IRequest<IActionResult>
    {
       public int Id { get; set; }

        public class DeleteNoteHandler : IRequestHandler<DeleteNoteCommand, IActionResult>
        {
            private readonly CourseContext _context;

            public DeleteNoteHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes.FindAsync(request.Id);

                if (note == null)
                {
                    return new BadRequestObjectResult(Message.MSG39);
                }
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(Message.MSG16);
            }
        }
    }
}
