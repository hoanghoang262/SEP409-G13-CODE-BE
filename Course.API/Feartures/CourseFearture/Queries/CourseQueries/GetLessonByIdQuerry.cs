using Contract.Service.Message;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetLessonByIdQuerry : IRequest<IActionResult>
    {
        public int LessonId { get; set; }
        public class GetLessonByIdQuerryHandler : IRequestHandler<GetLessonByIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public GetLessonByIdQuerryHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(GetLessonByIdQuerry request, CancellationToken cancellationToken)
            {
                var lesson = await _context.Lessons
                    .Include(l => l.Chapter)
                    .ThenInclude(c => c.Course)
                    .Include(les => les.TheoryQuestions)
                    .ThenInclude(ans => ans.AnswerOptions)
                    .FirstOrDefaultAsync(l => l.Id == request.LessonId);

                if (lesson == null)
                {
                    return new NotFoundObjectResult(lesson);
                }

                var lessonDTO = new LessonDTO
                {
                    Id = lesson.Id,
                    ChapterId = lesson.ChapterId,
                    ChapterName = lesson.Chapter.Name,
                    Description = lesson.Description,
                    Duration = lesson.Duration,
                    Title = lesson.Title,
                    VideoUrl = lesson.VideoUrl,
                    CourseName = lesson.Chapter.Course.Name,
                    ContentLesson= lesson.ContentLesson,
                    CodeForm=lesson.CodeForm,
                    TheoryQuestions = lesson.TheoryQuestions.Select(l => new TheoryQuestionDTO
                    {
                        Id = l.Id,
                        ContentQuestion = l.ContentQuestion,
                        Time = l.Time,
                        VideoId = l.VideoId,
                        TimeQuestion = l.TimeQuestion,
                        AnswerOptions = l.AnswerOptions.Select(c => new AnswerOptionDTO
                        {
                            Id = c.Id,
                            CorrectAnswer = c.CorrectAnswer,
                            OptionsText = c.OptionsText,
                            QuestionId = c.QuestionId
                        }).ToList()
                    }).ToList()
                };

                return new OkObjectResult(lessonDTO);
            }
        }
    }
}
