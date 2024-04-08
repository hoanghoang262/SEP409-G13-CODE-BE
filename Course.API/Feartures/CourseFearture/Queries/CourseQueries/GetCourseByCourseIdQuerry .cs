using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;
using CourseService.API.GrpcServices;
using Contract.Service.Message;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetCourseByCourseIdQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByCourseIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(CourseContext context, GetUserInfoService userIdCourseGrpcService, IMapper _mapper)
            {
                _context = context;
                service = userIdCourseGrpcService;
                mapper = _mapper;

            }
            public async Task<IActionResult> Handle(GetCourseByCourseIdQuerry request, CancellationToken cancellationToken)
            {
                var courses = await _context.Courses
                   
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                            .ThenInclude(l => l.TheoryQuestions)
                                .ThenInclude(ans => ans.AnswerOptions)
                     .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.LastExams)
                            .ThenInclude(l => l.QuestionExams)
                                .ThenInclude(ans => ans.AnswerExams)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                            .ThenInclude(cq => cq.TestCases)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                           
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                            .ThenInclude(l => l.TheoryQuestions)
                                .ThenInclude(ans => ans.AnswerOptions)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                            .ThenInclude(cq => cq.TestCases)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                          
                    .FirstOrDefaultAsync(course => course.Id == request.CourseId);

                if (courses == null)
                {
                    return new NotFoundObjectResult(courses);
                }

                courses.Chapters.OrderBy(c => c.Part);

                var user = await service.SendUserId(courses.CreatedBy);
                bool isUserEnrolled = _context.Enrollments.Any(e => e.UserId == request.UserId && e.CourseId == request.CourseId);
                bool isInWishList = _context.Wishlists.Any(e => e.UserId == request.UserId && e.CourseId == request.CourseId);

                var result = new
                {
                    courses.Id,
                    courses.Name,
                    courses.Description,
                    courses.Picture,
                    courses.Tag,
                    courses.CreatedBy,
                    courses.CreatedAt,
                    courses.Price,
                    Created_Name = user.Name,
                    Avatar = user.Picture,
                    IsEnrolled= isUserEnrolled ,
                    InWishList= isInWishList,

                    Chapters = courses.Chapters.Select(chapter => new
                    {
                        chapter.Id,
                        chapter.Name,
                        chapter.CourseId,
                        chapter.Part,
                        chapter.IsNew,
                        IsCompleted = AreAllLessonsCompleted(chapter, request.UserId),
                        LastExam = chapter.LastExams.Select(lastex => new
                        {
                            lastex.Id,
                            lastex.Name,
                            lastex.PercentageCompleted,
                            lastex.ChapterId,
                            IsCompleted = AreLastExamCompleted(lastex, request.UserId),
                            QuestionExams = lastex.QuestionExams.Select(qe => new
                            {
                                qe.Id,
                                qe.LastExamId,
                                qe.ContentQuestion,
                                qe.Score,
                                qe.Status,
                                AnswerExams = qe.AnswerExams.Select(ans => new
                                {
                                    ans.Id,
                                    ans.ExamId,
                                    ans.CorrectAnswer,
                                    ans.OptionsText

                                }).ToList(),

                            }).ToList(),

                        }).ToList(),
                        CodeQuestions = chapter.PracticeQuestions.Select(codeQuestion => new
                         {
                             codeQuestion.Id,
                             codeQuestion.Description,
                             codeQuestion.CodeForm,
                             codeQuestion.TestCaseC,
                             codeQuestion.TestCaseJava,
                             codeQuestion.TestCaseCplus,
                             codeQuestion.Title,
                             TestCases = codeQuestion.TestCases.Select(testCase => new
                             {
                                 testCase.Id,
                                 testCase.InputTypeInt,
                                 testCase.InputTypeString,
                                 testCase.ExpectedResultInt,
                                 testCase.CodeQuestionId,
                                 testCase.ExpectedResultString,
                                 testCase.InputTypeBoolean,
                                 testCase.ExpectedResultBoolean,
                                 testCase.InputTypeArrayInt,
                                 testCase.InputTypeArrayString
                             }).ToList(),
      
                         }).ToList(),
                        Lessons = chapter.Lessons.Select(lesson => new
                        {
                            lesson.Id,
                            lesson.Title,
                            lesson.VideoUrl,
                            lesson.ChapterId,
                            lesson.Description,
                            lesson.Duration,
                            lesson.ContentLesson,
                            lesson.CodeForm,
                            IsCompleted = AreLessonCompleted(lesson, request.UserId),

                            Questions = lesson.TheoryQuestions.Select(question => new
                            {
                                question.Id,
                                question.VideoId,
                                question.ContentQuestion,
                                question.Time,
                                AnswerOptions = question.AnswerOptions.Select(answerOption => new
                                {
                                    answerOption.Id,
                                    answerOption.QuestionId,
                                    answerOption.OptionsText,
                                    answerOption.CorrectAnswer
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };

                return new OkObjectResult(result);
            }
            private bool AreAllLessonsCompleted(Chapter chapter, int userId)
            {
                
                var lessons =  _context.Lessons.Where(lesson => lesson.ChapterId == chapter.Id).ToList();

              
                return lessons.All(lesson =>
                    _context.CompleteLessons.Any(cl => cl.UserId == userId && cl.LessonId == lesson.Id)
                );
            }
            private bool AreLessonCompleted(Lesson lesson, int userId)
            {
                return _context.CompleteLessons.Any(cl => cl.UserId == userId && cl.LessonId == lesson.Id);
            }
            private bool AreLastExamCompleted(LastExam last, int userId)
            {
                return _context.CompletedExams.Any(cl => cl.UserId == userId && cl.LastExamId == last.Id);
            }
          

        }
    }
}