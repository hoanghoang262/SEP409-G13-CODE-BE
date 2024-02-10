using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.UpdateCourse.Chapter
{
    public class CreateChapterCommand : IRequest<IActionResult>
    {
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        public class CreateChapterCommandHandler : IRequestHandler<CreateChapterCommand, IActionResult>
        {
            private readonly CourseContext _context;

            public CreateChapterCommandHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
            {

                return new OkObjectResult("lalala");
                
            }
        }
    }
}
