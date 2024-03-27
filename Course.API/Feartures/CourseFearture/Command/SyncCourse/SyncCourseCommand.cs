using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.Event;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncCourseCommand : IRequest<IActionResult>, IMapFrom<CourseEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Price { get; set; }
        public class asyncCourseCommandHandler : IRequestHandler<SyncCourseCommand, IActionResult>
        {
            private readonly CourseContext _context;
            

            public asyncCourseCommandHandler(CourseContext context)
            {
                _context = context;
            
            }
            public async Task<IActionResult> Handle(SyncCourseCommand request, CancellationToken cancellationToken)
            {
                
                var courseDelete = await _context.Courses
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
                            
                      .FirstOrDefaultAsync(course => course.Id == request.Id);

                if (courseDelete != null)
                {

                    foreach (var chapter in courseDelete.Chapters)
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

                    _context.Remove(courseDelete);
                 
                }


                var course = await _context.Courses.FindAsync(request.Id);
                if (course == null)
                {
                    var newCourse = new Course
                    {
                        Id = request.Id,
                        Name = request.Name,
                        Description = request.Description,
                        Picture = request.Picture,
                        Tag = request.Tag,
                        CreatedBy = request.CreatedBy,
                        CreatedAt=request.CreatedAt,
                        Price = request.Price,  

                    };

                    _context.Courses.Add(newCourse);

                }
                else
                {

                    course.Name = request.Name;
                    course.Description = request.Description;
                    course.Picture = request.Picture;
                    course.Tag = request.Tag;
                    course.CreatedBy = request.CreatedBy;
                    course.Price = request.Price;


                  
                }
                await _context.SaveChangesAsync(cancellationToken);





                return new OkObjectResult("done");
            }
        }
    }

}
