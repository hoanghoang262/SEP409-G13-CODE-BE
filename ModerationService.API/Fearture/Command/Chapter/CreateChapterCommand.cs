using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateChapterCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
    }

    public class AddChapterCommandHandler : IRequestHandler<CreateChapterCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public AddChapterCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = new Chapter
            {
                Name = request.Name,
                CourseId = request.CourseId,
                Part = request.Part,
                IsNew = request.IsNew
            };

            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            return chapter.Id;
        }

    }
}
