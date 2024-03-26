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
            private readonly Content_ModerationContext _context;

            public DeleteCourseCommandHandler(Content_ModerationContext moderationContext)
            {
                _context = moderationContext;
            }
            public async Task<IActionResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
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
                          .ThenInclude(ch => ch.LastExams)
                              .ThenInclude(l => l.QuestionExams)
                                  .ThenInclude(ans => ans.AnswerExams)
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

                if (course != null)
                {
                   
                    foreach (var chapter in course.Chapters)
                    {
                        foreach (var lesson in chapter.Lessons)
                        {
                            foreach (var question in lesson.TheoryQuestions)
                            {
                                _context.RemoveRange(question.AnswerOptions);
                                _context.Remove(question);
                            }
                            _context.Remove(lesson);
                        }

                        foreach (var practiceQuestion in chapter.PracticeQuestions)
                        {
                            _context.RemoveRange(practiceQuestion.TestCases);
                            _context.RemoveRange(practiceQuestion.UserAnswerCodes);
                            _context.Remove(practiceQuestion);
                        }

                        foreach (var lastExam in chapter.LastExams)
                        {
                            foreach (var questionExam in lastExam.QuestionExams)
                            {
                                _context.Remove(questionExam.AnswerExams);
                                _context.Remove(questionExam);
                            }
                            _context.Remove(lastExam);
                        }

                        _context.Remove(chapter);
                    }

                    _context.Remove(course); 
                    await _context.SaveChangesAsync(); 
                }

                return new OkObjectResult(course.Id);
            }
        }
    }
}
