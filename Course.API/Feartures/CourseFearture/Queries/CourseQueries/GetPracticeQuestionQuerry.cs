using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetPracticeQuestionByIdQuerry : IRequest<PracticeQuestionDTO>
    {
        public int PracticeQuestionId { get; set; }

        public class GetPracticeQuestionQuerryHandler : IRequestHandler<GetPracticeQuestionByIdQuerry, PracticeQuestionDTO>
        {
            private readonly Course_DeployContext _context;
            public GetPracticeQuestionQuerryHandler(Course_DeployContext context)
            {
                _context = context;
            }

            public async Task<PracticeQuestionDTO> Handle(GetPracticeQuestionByIdQuerry request, CancellationToken cancellationToken)
            {
                var practiceQuestion = await _context.PracticeQuestions
                                              .Include(pq => pq.Chapter)
                                              .ThenInclude(c=>c.Course)
                                              .Include(pq => pq.TestCases)
                                              .Include(pq => pq.UserAnswerCodes)
                                              .FirstOrDefaultAsync(pq => pq.Id == request.PracticeQuestionId);

                if (practiceQuestion == null)
                {
                    return null;
                }

                var practiceQuestionDTO = new PracticeQuestionDTO
                {
                    Id = practiceQuestion.Id,
                    Description = practiceQuestion.Description,
                    ChapterId = practiceQuestion.ChapterId,
                    CodeForm = practiceQuestion.CodeForm,
                    TestCaseJava = practiceQuestion.TestCaseJava,
                    ChapterName = practiceQuestion.Chapter.Name,
                    CourseName=practiceQuestion.Chapter.Course.Name,
                    TestCases = practiceQuestion.TestCases.Select(tc => new TestCase
                    {
                        Id = tc.Id,
                        InputTypeInt = tc.InputTypeInt,
                        InputTypeString = tc.InputTypeString,
                        ExpectedResultInt = tc.ExpectedResultInt,
                        CodeQuestionId = tc.CodeQuestionId,
                        ExpectedResultString = tc.ExpectedResultString,
                        InputTypeBoolean = tc.InputTypeBoolean,
                        ExpectedResultBoolean = tc.ExpectedResultBoolean,
                        InputTypeArrayInt = tc.InputTypeArrayInt,
                        InputTypeArrayString = tc.InputTypeArrayString
                    }).ToList(),
                    UserAnswerCodes = practiceQuestion.UserAnswerCodes.Select(uac => new UserAnswerCode
                    {
                        Id = uac.Id,
                        AnswerCode=uac.AnswerCode,
                        CodeQuestionId=uac.CodeQuestionId,
                        UserId= uac.UserId  
                    }).ToList()
                };

                return practiceQuestionDTO;
               
            }
        }
    }
}
