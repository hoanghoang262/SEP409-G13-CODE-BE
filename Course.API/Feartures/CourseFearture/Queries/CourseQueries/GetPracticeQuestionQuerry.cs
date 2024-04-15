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
        public int UserId { get; set; }

        public class GetPracticeQuestionQuerryHandler : IRequestHandler<GetPracticeQuestionByIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public GetPracticeQuestionQuerryHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(GetPracticeQuestionByIdQuerry request, CancellationToken cancellationToken)
            {
                var practiceQuestion = await (from c in _context.Courses
                                              join ch in _context.Chapters on c.Id equals ch.CourseId
                                              join pr in _context.PracticeQuestions on ch.Id equals pr.ChapterId
                                              join ans in _context.UserAnswerCodes on pr.Id equals ans.CodeQuestionId into ansGroup
                                              from ans in ansGroup.DefaultIfEmpty() 
                                              where pr.Id == request.PracticeQuestionId
                                              select new
                                              {
                                                  PracticeQuestion = pr,
                                                  Chapter = ch,
                                                  Course = c,
                                                  UserAnswerCode = ans 
                                              }).FirstOrDefaultAsync();

                if (practiceQuestion == null)
                {
                    return new NotFoundObjectResult(practiceQuestion);
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
                    UserAnswer = _context.UserAnswerCodes
                      .Where(uac => uac.UserId == request.UserId && uac.CodeQuestionId == request.PracticeQuestionId)
                     .OrderByDescending(uac => uac.Id)
                      .Select(uac => uac.AnswerCode) 
                       .FirstOrDefault()
                };


                return new OkObjectResult(practiceQuestionDTO);
            }
        }
    }
}
