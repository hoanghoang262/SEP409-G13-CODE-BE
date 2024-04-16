using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateLessonCommand : IRequest<IActionResult>
    {
        public int ChapterId { get; set; }
        public LessonDTO Lesson { get; set; }
    }

    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public CreateLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<IActionResult> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            // validate input
            if (string.IsNullOrEmpty(request.Lesson.Title)
                || string.IsNullOrEmpty(request.Lesson.VideoUrl)
                || string.IsNullOrEmpty(request.Lesson.Description)
                || request.Lesson.Duration == null
                || string.IsNullOrEmpty(request.Lesson.ContentLesson))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }
            if (request.Lesson.Duration < 0)
            {
                return new BadRequestObjectResult(Message.MSG26);
            }
            foreach (var item in request.Lesson.Questions)
            {
                if (item.Time < 0)
                {
                    return new BadRequestObjectResult(Message.MSG26);
                }
            }

            var chapter = await _context.Chapters
                 .Include(c => c.Lessons)
                     .ThenInclude(l => l.TheoryQuestions)
                         .ThenInclude(tq => tq.AnswerOptions)
                 .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

            // check if chapter exists
            if (chapter == null)
            {
                return new BadRequestObjectResult(Message.MSG28);
            }

            var newLesson = new Lesson
            {
                ChapterId = request.ChapterId,
                Title = request.Lesson.Title,
                VideoUrl = request.Lesson.VideoUrl,
                Description = request.Lesson.Description,
                Duration = request.Lesson.Duration,
                ContentLesson = request.Lesson.ContentLesson,
                CodeForm = request.Lesson.CodeForm,
            };

            chapter.Lessons.Add(newLesson);

            foreach (var theoryQuestionDTO in request.Lesson.Questions)
            {
                var newTheoryQuestion = new TheoryQuestion
                {
                    VideoId = newLesson.Id,
                    ContentQuestion = theoryQuestionDTO.ContentQuestion,
                    Time = theoryQuestionDTO.Time
                };

                foreach (var answerOptionDTO in theoryQuestionDTO.AnswerOptions)
                {
                    var newAnswerOption = new AnswerOption
                    {
                        QuestionId = newTheoryQuestion.Id,
                        OptionsText = answerOptionDTO.OptionsText,
                        CorrectAnswer = answerOptionDTO.CorrectAnswer
                    };

                    newTheoryQuestion.AnswerOptions.Add(newAnswerOption);
                }

                newLesson.TheoryQuestions.Add(newTheoryQuestion);
            }

            await _context.SaveChangesAsync();
            var lessonDTO = new LessonDTO
            {
                Id = newLesson.Id,
                ChapterId = newLesson.ChapterId,
                Title = newLesson.Title,
                VideoUrl = newLesson.VideoUrl,
                Description = newLesson.Description,
                Duration = newLesson.Duration,
                ContentLesson = newLesson.ContentLesson,
                CodeForm = newLesson.CodeForm,
                Questions = newLesson.TheoryQuestions.Select(tq => new TheoryQuestionDTO
                {
                    Id = tq.Id,
                    VideoId = tq.VideoId,
                    ContentQuestion = tq.ContentQuestion,
                    Time = tq.Time,
                    AnswerOptions = tq.AnswerOptions.Select(ao => new AnswerOptionsDTO
                    {
                        Id = ao.Id,
                        QuestionId = ao.QuestionId,
                        OptionsText = ao.OptionsText,
                        CorrectAnswer = ao.CorrectAnswer
                    }).ToList()
                }).ToList()
            };

            return new OkObjectResult(lessonDTO);
        }
    }

}
