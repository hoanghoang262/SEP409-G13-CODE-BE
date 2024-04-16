using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Course
{
    public class DeleteCourseCommand : IRequest<IActionResult>
    {
        public int CourseId { get; set; }

        public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, IActionResult>
        {
            private readonly Content_ModerationContext context;

            public DeleteCourseCommandHandler(Content_ModerationContext moderationContext)
            {
                context = moderationContext;
            }
            public async Task<IActionResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
            {
               
                var courseToRemove = context.Courses
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                            .ThenInclude(l => l.TheoryQuestions)
                                .ThenInclude(ans => ans.AnswerOptions)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                            .ThenInclude(cq => cq.TestCases)
                      .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.LastExams)
                         .ThenInclude(ch => ch.QuestionExams)
                          .ThenInclude(ch => ch.AnswerExams)
                    .FirstOrDefault(course => course.Id == request.CourseId);

                if (courseToRemove == null)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }

                foreach (var chapter in courseToRemove.Chapters)
                {
                    foreach (var lesson in chapter.Lessons)
                    {
                        foreach (var theoryQuestion in lesson.TheoryQuestions)
                        {

                            context.RemoveRange(theoryQuestion.AnswerOptions);
                        }

                        context.RemoveRange(lesson.TheoryQuestions);
                    }
                }


                foreach (var chapter in courseToRemove.Chapters)
                {
                    foreach (var practiceQuestion in chapter.PracticeQuestions)
                    {

                        context.RemoveRange(practiceQuestion.TestCases);
                    }

                    context.RemoveRange(chapter.PracticeQuestions);
                }

               
                foreach (var chapter in courseToRemove.Chapters)
                {
                    foreach (var lastExam in chapter.LastExams)
                    {
                        foreach (var questionExam in lastExam.QuestionExams)
                        {
                            
                            context.RemoveRange(questionExam.AnswerExams);
                        }
                     
                        context.RemoveRange(lastExam.QuestionExams);
                    }
                 
                    context.RemoveRange(chapter.LastExams);
                }

                context.RemoveRange(courseToRemove.Chapters);

                context.Remove(courseToRemove);

              
                context.SaveChanges();
            
                var moderation = context.Moderations.FirstOrDefault(c => c.CourseId.Equals(request.CourseId));
                if (moderation != null)
                {
                    context.Moderations.Remove(moderation);
                    await context.SaveChangesAsync();
                }

                return new OkObjectResult(courseToRemove.Id);
            }
        }
    }
}
