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
    public class SyncChapterCommand : IRequest<IActionResult>, IMapFrom<ChapterEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
        public class asyncChapterHandler : IRequestHandler<SyncChapterCommand, IActionResult>
        {
            private readonly CourseContext _context;


            public asyncChapterHandler(CourseContext context)
            {
                _context = context;

            }
            public async Task<IActionResult> Handle(SyncChapterCommand request, CancellationToken cancellationToken)
            {
                var c = _context.Chapters.FirstOrDefault(c => c.Id.Equals(request.Id));
                if (c != null)
                {
                    var chapterDelete = _context.Chapters

                      .Include(ch => ch.Lessons)
                          .ThenInclude(l => l.TheoryQuestions)
                              .ThenInclude(ans => ans.AnswerOptions)

                      .Include(ch => ch.PracticeQuestions)
                          .ThenInclude(cq => cq.TestCases)

                      .Include(ch => ch.LastExams)
                          .ThenInclude(l => l.QuestionExams)
                              .ThenInclude(ans => ans.AnswerExams)

                      .Include(ch => ch.PracticeQuestions)
                          .ThenInclude(cq => cq.TestCases)
                  .FirstOrDefault(course => course.Id == request.Id);



                    foreach (var lesson in chapterDelete.Lessons)
                    {
                        foreach (var question in lesson.TheoryQuestions)
                        {
                            _context.AnswerOptions.RemoveRange(question.AnswerOptions);
                            _context.TheoryQuestions.Remove(question);
                        }
                        _context.Lessons.Remove(lesson);
                    }

                    foreach (var practiceQuestion in chapterDelete.PracticeQuestions)
                    {
                        _context.TestCases.RemoveRange(practiceQuestion.TestCases);

                        _context.PracticeQuestions.Remove(practiceQuestion);
                    }

                    foreach (var lastExam in chapterDelete.LastExams)
                    {
                        foreach (var questionExam in lastExam.QuestionExams)
                        {
                            _context.AnswerExams.RemoveRange(questionExam.AnswerExams);
                            _context.QuestionExams.Remove(questionExam);
                        }
                        _context.LastExams.Remove(lastExam);
                    }

                    _context.Chapters.Remove(chapterDelete);

                     _context.SaveChanges();

                }
             

                var chapter = await _context.Chapters.FindAsync(request.Id);
                if (chapter == null)
                {
                    var newChapter = new Chapter
                    {
                        Id = request.Id,
                        Name = request.Name,
                        CourseId = request.CourseId,
                        Part = request.Part,
                        IsNew = request.IsNew

                    };

                    _context.Chapters.Add(newChapter);
                     _context.SaveChanges();

                }
                else
                {
                    chapter.Name = request.Name;
                    chapter.CourseId = request.CourseId;
                    chapter.Part = request.Part;
                    chapter.IsNew = true;
                    await _context.SaveChangesAsync(cancellationToken);

                }




                return new OkObjectResult("done");
            }
        }
    }

}
