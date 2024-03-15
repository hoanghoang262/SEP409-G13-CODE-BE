using CourseService.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.Notes
{
    public class GetAllNoteOfUserQuerry : IRequest<List<Note>>
    {
        public int UserId { get; set; } 
        public int LessonId { get; set; }
        public class GetAllNoteOfUserQuerryHandler : IRequestHandler<GetAllNoteOfUserQuerry, List<Note>>
        {
            private readonly CourseContext _context;

            public GetAllNoteOfUserQuerryHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<List<Note>> Handle(GetAllNoteOfUserQuerry request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes.Where(x=>x.UserId.Equals(request.UserId)&& x.LessonId.Equals(request.LessonId)).ToListAsync();
                if(note== null)
                {
                    return null;
                }
                return note;
            }
        }
    }
}
