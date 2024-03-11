using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class UpdateChapterCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
    }

    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public UpdateChapterCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters.FindAsync(request.Id);

            if (chapter == null)
                return 0;

            chapter.Name = request.Name;
            chapter.CourseId = request.CourseId;
            chapter.Part = request.Part;
            chapter.IsNew = request.IsNew;

            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();

            return chapter.Id;
        }
    }
}
