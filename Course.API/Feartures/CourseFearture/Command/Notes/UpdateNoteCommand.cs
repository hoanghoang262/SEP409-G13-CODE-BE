using Contract.Service.Message;
using CourseService.API.GrpcServices;
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
            private readonly GetUserInfoService _service;

            public UpdateNoteHandler(CourseContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
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
                var user = await _service.SendUserId(request.UserId);
                if(user == null) 
                {
                    return new BadRequestObjectResult(Message.MSG01);
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
