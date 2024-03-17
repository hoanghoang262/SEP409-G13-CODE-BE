using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Lesson
{
    public class GetLessonByIdQuery : IRequest<LessonDTO>
    {
        public int LessonId { get; set; }
    }
    public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, LessonDTO>
    {
        private readonly Content_ModerationContext _context;

        public GetLessonByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<LessonDTO> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            var lesson = await _context.Lessons.Include(c=>c.TheoryQuestions).ThenInclude(a=>a.AnswerOptions).FirstOrDefaultAsync(l => l.Id == request.LessonId);

            if (lesson == null)
            {
                return null; 
            }

            var lessonDTO = new LessonDTO
            {
                Id = lesson.Id,
                Title = lesson.Title,
                VideoUrl = lesson.VideoUrl,
                ChapterId = lesson.ChapterId,
                Description = lesson.Description,
                IsCompleted = lesson.IsCompleted,
                Duration = lesson.Duration,
                ContentLesson = lesson.ContentLesson,
                Questions = lesson.TheoryQuestions.Select(n => new TheoryQuestionDTO
                {
                    Id = n.Id,
                    ContentQuestion = n.ContentQuestion,
                    Time = n.Time,
                    TimeQuestion = n.TimeQuestion,
                    VideoId = n.VideoId,
                    AnswerOptions = n.AnswerOptions.Select(a => new AnswerOptionsDTO
                    {
                        Id=a.Id,
                        OptionsText=a.OptionsText,
                        CorrectAnswer=a.CorrectAnswer,
                        QuestionId = a.QuestionId   

                    }).ToList()

                }).ToList()
            };

            return lessonDTO;
        }
    }
}
