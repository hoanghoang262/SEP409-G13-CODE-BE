using Contract.Service.Message;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetChapterByIdQuerry : IRequest<IActionResult>
    {
        public int ChapterId { get; set; }
        public int UserId { get; set; }

        public class GetChapterByIdQuerryHandler : IRequestHandler<GetChapterByIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public GetChapterByIdQuerryHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(GetChapterByIdQuerry request, CancellationToken cancellationToken)
            {
                var chapter = await _context.Chapters.Include(c => c.Lessons).Include(pc => pc.PracticeQuestions).FirstOrDefaultAsync(c => c.Id.Equals(request.ChapterId));

                if (chapter == null)
                {
                    return new NotFoundObjectResult(chapter);
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
                        IsCompleted =  IsLessonCompleted(lesson.Id, request.UserId)
                    }).ToList(),
                    PracticeQuestions = chapter.PracticeQuestions.Select(practiceQuestion => new PracticeQuestionDTO
                    {
                        Id = practiceQuestion.Id,
                        Description = practiceQuestion.Description,
                        ChapterId = practiceQuestion.ChapterId,
                        CodeForm = practiceQuestion.CodeForm,
                    }).ToList()
                };

                return new OkObjectResult(chapterDTO);
            }
            private bool IsLessonCompleted(int lessonId, int userId)
            {
                return  _context.CompleteLessons
                                    .Any(cl => cl.LessonId == lessonId && cl.UserId == userId);
            }
        }
      
    }
}
