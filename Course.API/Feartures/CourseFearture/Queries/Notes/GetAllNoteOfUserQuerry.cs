using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.Notes
{
    public class GetAllNoteOfUserQuerry : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public class GetAllNoteOfUserQuerryHandler : IRequestHandler<GetAllNoteOfUserQuerry, IActionResult>
        {
            private readonly CourseContext _context;

            public GetAllNoteOfUserQuerryHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(GetAllNoteOfUserQuerry request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes
                    .Where(x => x.UserId.Equals(request.UserId) && x.LessonId.Equals(request.LessonId)).ToListAsync();
                if (note == null)
                {
                    return new NotFoundObjectResult(Message.MSG22);
                }

                return new OkObjectResult(note);
            }
        }
    }
}
