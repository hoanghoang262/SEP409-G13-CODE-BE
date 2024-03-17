using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Course
{
    public class DeleteCourseCommand : IRequest<ActionResult<int>>
    {
        public int CourseId { get; set; }

        public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, ActionResult<int>>
        {
            private readonly Content_ModerationContext _context;

            public DeleteCourseCommandHandler(Content_ModerationContext moderationContext)
            {
                _context = moderationContext;
            }
            public async Task<ActionResult<int>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await _context.Courses
                      .Include(c => c.Chapters)
                          .ThenInclude(ch => ch.Lessons)
                              .ThenInclude(l => l.TheoryQuestions)
                                  .ThenInclude(ans => ans.AnswerOptions)
                      .Include(c => c.Chapters)
                          .ThenInclude(ch => ch.PracticeQuestions)
                              .ThenInclude(cq => cq.TestCases)
                      .Include(c => c.Chapters)
                          .ThenInclude(ch => ch.Lessons)
                              .ThenInclude(l => l.TheoryQuestions)
                                  .ThenInclude(ans => ans.AnswerOptions)
                      .Include(c => c.Chapters)
                          .ThenInclude(ch => ch.PracticeQuestions)
                              .ThenInclude(cq => cq.TestCases)
                      .Include(c => c.Chapters)
                          .ThenInclude(ch => ch.PracticeQuestions)
                              .ThenInclude(cq => cq.UserAnswerCodes)
                      .FirstOrDefaultAsync(course => course.Id == request.CourseId);

                if (course == null)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }

                foreach (var chapter in course.Chapters)
                {
                    foreach (var practiceQuestion in chapter.PracticeQuestions)
                    {
                        _context.TestCases.RemoveRange(practiceQuestion.TestCases);
                    }
                }
                foreach (var chapter in course.Chapters)
                {
                    foreach (var lesson in chapter.Lessons)
                    {
                        _context.TheoryQuestions.RemoveRange(lesson.TheoryQuestions);
                    }
                }
                foreach (var chapter in course.Chapters)
                {
                    _context.PracticeQuestions.RemoveRange(chapter.PracticeQuestions);
                }
                foreach (var chapter in course.Chapters)
                {
                    _context.Lessons.RemoveRange(chapter.Lessons);
                }

                _context.Chapters.RemoveRange(course.Chapters);
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return new OkObjectResult(course.Id);
            }
        }
    }
}
