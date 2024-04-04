using Contract.Service.Message;
using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetModerationCourseByIdQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }

        public class GetModerationCourseByIdHandler : IRequestHandler<GetModerationCourseByIdQuerry, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetUserInfoService _service;

            public GetModerationCourseByIdHandler(Content_ModerationContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(GetModerationCourseByIdQuerry request, CancellationToken cancellationToken)
            {
                var courses = await _context.Courses.Include(c=>c.Moderations)
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
                    return new NotFoundObjectResult(courses);
                }

                courses.Chapters.OrderBy(c => c.Part);
                var user = await _service.SendUserId((int)courses.CreatedBy);

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
                    Avatar = user.Picture,
                    Chapters = courses.Chapters.Select(chapter => new
                    {
                        chapter.Id,
                        chapter.Name,
                        chapter.CourseId,
                        chapter.Part,
                        chapter.IsNew,
                        LastExam = chapter.LastExams.Select(lastex => new
                        {
                            lastex.Id,
                            lastex.Name,
                            lastex.PercentageCompleted,
                            
                            lastex.ChapterId,
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
                            lesson.CodeForm,
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
