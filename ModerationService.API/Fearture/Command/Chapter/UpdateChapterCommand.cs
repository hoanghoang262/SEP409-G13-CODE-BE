using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class UpdateChapterCommand : IRequest<Chapter>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
       
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
    }

    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, Chapter>
    {
        private readonly Content_ModerationContext _context;

        public UpdateChapterCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<Chapter> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters.FindAsync(request.Id);

            if (chapter == null)
                return null;

            chapter.Name = request.Name;
         
            chapter.Part = request.Part;
            chapter.IsNew = request.IsNew;

            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();

            return chapter;
        }
    }
}
