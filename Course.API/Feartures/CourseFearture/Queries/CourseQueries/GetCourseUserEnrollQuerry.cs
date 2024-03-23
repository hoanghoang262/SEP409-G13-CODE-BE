using Contract.Service.Message;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetCourseUserEnrollQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
    public class GetCourseUserEnrollQuerryHandler : IRequestHandler<GetCourseUserEnrollQuerry, IActionResult>
    {
        private readonly CourseContext _context;
        private readonly GetUserInfoService service;

        public GetCourseUserEnrollQuerryHandler(CourseContext context, GetUserInfoService service)
        {
            _context = context;
            this.service = service;
        }
        public async Task<IActionResult> Handle(GetCourseUserEnrollQuerry request, CancellationToken cancellationToken)
        {
            var courses = await _context.Enrollments
                                         .Include(c => c.Course)
                                           .ThenInclude(c => c.Chapters)
                                             .ThenInclude(ch => ch.Lessons)
                                               .ThenInclude(l => l.TheoryQuestions)
                                                 .ThenInclude(ans => ans.AnswerOptions)
                                                 .Include(c=>c.Course)
                                                  .ThenInclude(c => c.Chapters)
                                                     .ThenInclude(ch => ch.Lessons)
                                                       .ThenInclude(cq => cq.Notes)
                                            .Include(c => c.Course)
                                              .ThenInclude(c => c.Chapters)
                                                .ThenInclude(ch => ch.PracticeQuestions)
                                                  .ThenInclude(cq => cq.TestCases)
                                             .Include(c => c.Course)
                                               .ThenInclude(c => c.Chapters)
                                                 .ThenInclude(ch => ch.Lessons)
                                                   .ThenInclude(l => l.TheoryQuestions)
                                                     .ThenInclude(ans => ans.AnswerOptions)
                                             .Include(c => c.Course)
                                               .ThenInclude(c => c.Chapters)
                                                 .ThenInclude(ch => ch.PracticeQuestions)
                                                   .ThenInclude(cq => cq.TestCases)
                                             .Include(c => c.Course)
                                               .ThenInclude(c => c.Chapters)
                                                 .ThenInclude(ch => ch.PracticeQuestions)
                                                   .ThenInclude(cq => cq.UserAnswerCodes)
                                                  .FirstOrDefaultAsync(enroll => enroll.CourseId == request.CourseId && enroll.UserId == request.UserId);

            var user = await service.SendUserId(courses.Course.CreatedBy);
            if (courses == null || user == null)
            {
                return new NotFoundObjectResult(Message.MSG22);
            }

            var result = new
            {
                courses.Course.Id,
                courses.Course.Name,
                courses.Course.Description,
                courses.Course.Picture,
                courses.Course.Tag,
                courses.Course.CreatedBy,
                courses.Course.CreatedAt,
                Created_Name = user.Name,
                Avatar = user.Picture,
                Chapters = courses.Course.Chapters.Select(chapter => new
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
                        Notes = lesson.Notes.Select(note => new
                        {
                            note.VideoLink,
                            note.ContentNote,

                        }).ToList(),
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
