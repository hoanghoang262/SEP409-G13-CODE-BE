using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Chapter
{
    public class GetChapterByIdQuery : IRequest<ChapterDTO>
    {
        public int ChapterId { get; set; }
    }
    public class GetChapterByIdQueryHandler : IRequestHandler<GetChapterByIdQuery, ChapterDTO>
    {
        private readonly Content_ModerationContext _context;

        public GetChapterByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<ChapterDTO> Handle(GetChapterByIdQuery request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == request.ChapterId);

            if (chapter == null)
            {
                return null;
            }

            
            var chapterDTO = new ChapterDTO
            {
                Id = chapter.Id,
                Name = chapter.Name,
                CourseId = chapter.CourseId,
                Part = chapter.Part,
                IsNew = chapter.IsNew
            };

            return chapterDTO;
        }
    }
}
