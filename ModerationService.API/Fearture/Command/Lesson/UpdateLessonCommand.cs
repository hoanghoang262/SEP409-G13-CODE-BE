using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class UpdateLessonCommand : IRequest<ActionResult<LessonDTO>>
    {
        public int LessonId { get; set; }
        public LessonDTO Lesson { get; set; }
    }

    public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, ActionResult<LessonDTO>>
    {
        private readonly Content_ModerationContext _context;

        public UpdateLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<ActionResult<LessonDTO>> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
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

            var existingLesson = await _context.Lessons
                .Include(l => l.TheoryQuestions)
                    .ThenInclude(tq => tq.AnswerOptions)
                .FirstOrDefaultAsync(l => l.Id == request.LessonId);

            // Check if lesson exists
            if (existingLesson == null)
            {
                return new BadRequestObjectResult(Message.MSG29);
            }

            existingLesson.Title = request.Lesson.Title;
            existingLesson.VideoUrl = request.Lesson.VideoUrl;
            existingLesson.Description = request.Lesson.Description;
            existingLesson.Duration = request.Lesson.Duration;
            existingLesson.ContentLesson = request.Lesson.ContentLesson;

            // Clear existing questions and options
            existingLesson.TheoryQuestions.Clear();

            // Add new questions and options
            foreach (var theoryQuestionDTO in request.Lesson.Questions)
            {
                var newTheoryQuestion = new TheoryQuestion
                {
                    VideoId = existingLesson.Id,
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

                existingLesson.TheoryQuestions.Add(newTheoryQuestion);
            }

            await _context.SaveChangesAsync();

            var lessonDTO = new LessonDTO
            {
                Id = existingLesson.Id,
                ChapterId = existingLesson.ChapterId,
                Title = existingLesson.Title,
                VideoUrl = existingLesson.VideoUrl,
                Description = existingLesson.Description,
                Duration = existingLesson.Duration,
                ContentLesson = existingLesson.ContentLesson,
                Questions = existingLesson.TheoryQuestions.Select(tq => new TheoryQuestionDTO
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

            return lessonDTO;
        }
    }
}
