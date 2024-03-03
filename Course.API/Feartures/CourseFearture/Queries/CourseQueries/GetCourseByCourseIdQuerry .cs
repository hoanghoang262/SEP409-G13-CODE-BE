using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
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
                var courses = await _context.Courses
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

                if (courses == null)
                {
                    return new NotFoundObjectResult("There is no course in here"); // Không tìm thấy khóa học
                }

                courses.Chapters.OrderBy(c => c.Part);
                var user = await service.SendUserId(courses.CreatedBy);

                var result = new
                {
                    courses.Id,
                    courses.Name,
                    courses.Description,
                    courses.Picture,
                    courses.Tag,
                    courses.CreatedBy,
                    courses.CreatedAt,
                    Created_Name = user.Name,
                    Chapters = courses.Chapters.Select(chapter => new
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
                            lesson.ContentLesson,
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

        }
    }
}