using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateLessonCommand : IRequest<int>
    {
        public int ChapterId { get; set; }
        public LessonDTO Lesson { get; set; }
    }

    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, int>
    {
        private readonly Content_ModerationContext _context;

        public CreateLessonCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<int> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _context.Chapters
                 .Include(c => c.Lessons)
                     .ThenInclude(l => l.TheoryQuestions)
                         .ThenInclude(tq => tq.AnswerOptions)
                 .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

            if (chapter != null)
            {
                
                foreach (var lesson in chapter.Lessons)
                {
                    foreach (var theoryQuestion in lesson.TheoryQuestions)
                    {
                        _context.AnswerOptions.RemoveRange(theoryQuestion.AnswerOptions);
                    }
                }

               
                _context.TheoryQuestions.RemoveRange(chapter.Lessons.SelectMany(l => l.TheoryQuestions));

               
                _context.Lessons.RemoveRange(chapter.Lessons);

                
                await _context.SaveChangesAsync();

               
            }
            var newLesson = new Lesson
            {
                ChapterId = request.ChapterId,
              
                Title = request.Lesson.Title,
                VideoUrl = request.Lesson.VideoUrl,
                Description = request.Lesson.Description,
                Duration = request.Lesson.Duration,
                ContentLesson = request.Lesson.ContentLesson
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
                        QuestionId= newTheoryQuestion.Id,
                        OptionsText = answerOptionDTO.OptionsText,
                        CorrectAnswer = answerOptionDTO.CorrectAnswer
                    };

                    newTheoryQuestion.AnswerOptions.Add(newAnswerOption);
                }

                newLesson.TheoryQuestions.Add(newTheoryQuestion);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return newLesson.Id;


           
        }
    }

}
