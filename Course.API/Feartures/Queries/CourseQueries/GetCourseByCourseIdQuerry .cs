using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;

namespace CourseService.API.Feartures.Queries.CourseQueries
{
    public class GetCourseByCourseIdQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByCourseIdQuerry, IActionResult>
        {
            private readonly Course_DeployContext _context;
            private readonly UserIdCourseGrpcService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(Course_DeployContext context, UserIdCourseGrpcService userIdCourseGrpcService, IMapper _mapper)
            {
                _context = context;
                service = userIdCourseGrpcService;
                mapper = _mapper;

            }
            public async Task<IActionResult> Handle(GetCourseByCourseIdQuerry request, CancellationToken cancellationToken)
            {
                var courses = _context.Courses
        .Include(c => c.Chapters)
            .ThenInclude(ch => ch.Lessons)
                .ThenInclude(l => l.TheoryQuestions)
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
                          .Where(course => course.Id == request.CourseId).ToList();

                courses.ForEach(course =>
                {
                    course.Chapters = course.Chapters.OrderBy(chapter => chapter.Part).ToList();
                });

                var result = courses.Select(course => new
                {
                    course.Id,
                    course.Name,
                    course.Description,
                    course.Picture,
                    course.Tag,
                    Chapters = course.Chapters.Select(chapter => new
                    {
                        chapter.Id,
                        chapter.Name,
                        chapter.CourseId,
                        chapter.Part,
                        chapter.IsNew,
                        CodeQuestions = chapter.PracticeQuestions.Select(codeQuestion => new
                        {
                            codeQuestion.Id,
                            codeQuestion.Description,
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
                            UserAnswerCodes = codeQuestion.UserAnswerCodes.Select(userAnswerCode => new
                            {
                                userAnswerCode.Id,
                                userAnswerCode.CodeQuestionId,
                                userAnswerCode.AnswerCode,
                                userAnswerCode.UserId
                            }).ToList()
                        }).ToList(),
                        Lessons = chapter.Lessons.Select(lesson => new
                        {
                            lesson.Id,
                            lesson.Title,
                            lesson.VideoUrl,
                            lesson.ChapterId,
                            lesson.Description,
                            lesson.Duration,
                           
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
                }).ToList();


                return new OkObjectResult(result);
            }
        }
    }
}