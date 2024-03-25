using Contract.Service.Message;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetPracticeQuestionByIdQuerry : IRequest<IActionResult>
    {
        public int PracticeQuestionId { get; set; }

        public class GetPracticeQuestionQuerryHandler : IRequestHandler<GetPracticeQuestionByIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public GetPracticeQuestionQuerryHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(GetPracticeQuestionByIdQuerry request, CancellationToken cancellationToken)
            {
                var practiceQuestion = await (from pq in _context.PracticeQuestions
                                              join chapter in _context.Chapters on pq.ChapterId equals chapter.Id
                                              join course in _context.Courses on chapter.CourseId equals course.Id
                                              join testCase in _context.TestCases on pq.Id equals testCase.CodeQuestionId into testCases
                                              join userAnswerCode in _context.UserAnswerCodes on pq.Id equals userAnswerCode.CodeQuestionId into userAnswerCodes
                                              where pq.Id == request.PracticeQuestionId
                                              select new
                                              {
                                                  PracticeQuestion = pq,
                                                  Chapter = chapter,
                                                  Course = course,
                                                  TestCases = testCases,
                                                  UserAnswerCodes = userAnswerCodes
                                              }).FirstOrDefaultAsync();



                if (practiceQuestion == null)
                {
                    return new NotFoundObjectResult(Message.MSG22);
                }

                var practiceQuestionDTO = new PracticeQuestionDTO
                {
                    Id = practiceQuestion.PracticeQuestion.Id,
                    Description = practiceQuestion.PracticeQuestion.Description,
                    ChapterId = practiceQuestion.Chapter.Id,
                    CodeForm = practiceQuestion.PracticeQuestion.CodeForm,
                    TestCaseJava = practiceQuestion.PracticeQuestion.TestCaseJava,
                    ChapterName = practiceQuestion.Chapter.Name,
                    CourseName = practiceQuestion.Course.Name,
                    UserAnswerCodes = practiceQuestion.UserAnswerCodes.Select(uac => new UserAnswerCode
                    {
                        Id = uac.Id,
                        AnswerCode = uac.AnswerCode,
                        CodeQuestionId = uac.CodeQuestionId,
                        UserId = uac.UserId
                    }).ToList()
                };


                return new OkObjectResult(practiceQuestionDTO);
            }
        }
    }
}
