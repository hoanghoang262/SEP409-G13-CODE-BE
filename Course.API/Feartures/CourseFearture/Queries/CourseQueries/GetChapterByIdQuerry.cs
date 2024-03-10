using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetChapterByIdQuerry : IRequest<ChapterDTO>
    {
        public int ChapterId { get; set; }

        public class GetChapterByIdQuerryHandler : IRequestHandler<GetChapterByIdQuerry, ChapterDTO>
        {
            private readonly Course_DeployContext _context;
            public GetChapterByIdQuerryHandler(Course_DeployContext context)
            {
                _context = context;
            }

            public async Task<ChapterDTO> Handle(GetChapterByIdQuerry request, CancellationToken cancellationToken)
            {
                var chapter = await _context.Chapters.Include(c=>c.Lessons).Include(pc=>pc.PracticeQuestions).FirstOrDefaultAsync(c => c.Id.Equals(request.ChapterId));

                if (chapter == null)
                {
                    return null;
                }

                var chapterDTO = new ChapterDTO
                {
                    Id = chapter.Id,
                    CourseId = chapter.CourseId,
                    Name = chapter.Name,
                    Part = chapter.Part,
                    IsNew = chapter.IsNew,
                    Lessons = chapter.Lessons.Select(lesson => new LessonDTO
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        VideoUrl = lesson.VideoUrl,
                        ChapterId = lesson.ChapterId,
                        Description = lesson.Description,
                        Duration = lesson.Duration,
                        ContentLesson = lesson.ContentLesson,
                    }).ToList(),
                    PracticeQuestions = chapter.PracticeQuestions.Select(practiceQuestion => new PracticeQuestionDTO
                    {
                        Id = practiceQuestion.Id,
                        Description = practiceQuestion.Description,
                        ChapterId = practiceQuestion.ChapterId,
                        CodeForm = practiceQuestion.CodeForm,
                    }).ToList()
                };

                return chapterDTO;
            }
        }
    }
}
