using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetExamQuestionDetailQuerry : IRequest<IActionResult>
    {
        public int LastExamId { get; set; }
        public int UserId { get; set; }

        public class GetExamQuestionDetailQuerryHandler : IRequestHandler<GetExamQuestionDetailQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public GetExamQuestionDetailQuerryHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(GetExamQuestionDetailQuerry request, CancellationToken cancellationToken)
            {
                var lastExam = await _context.LastExams.Include(c => c.QuestionExams).ThenInclude(c => c.AnswerExams).FirstOrDefaultAsync(c => c.Id.Equals(request.LastExamId));


                var result = new LastExamDTO
                {
                    Id = lastExam.Id,
                    ChapterId = (int)lastExam.ChapterId,
                    PercentageCompleted = lastExam.PercentageCompleted,
                    Name = lastExam.Name,
                    Time = lastExam.Time,
                    IsPass=IsPassExam(request.UserId,request.LastExamId),

                    QuestionExams = lastExam.QuestionExams.Select(q => new QuestionExamDTO
                    {
                        Id = q.Id,
                        ContentQuestion = q.ContentQuestion,
                        LastExamId = (int)q.LastExamId,
                        Score = q.Score,
                        Status = q.Status,
                        AnswerExams = q.AnswerExams.Select(q => new AnswerExamDTO
                        {
                            ExamId = q.ExamId,
                            CorrectAnswer = q.CorrectAnswer,
                            Id = q.Id,
                            OptionsText = q.OptionsText

                        }).ToList(),
                    }).ToList(),
                };
                return new OkObjectResult(result);
            }
            public bool IsPassExam(int UserId, int LastExamId)
            {
                var ispass = _context.LastExams.FirstOrDefault(e => e.Id.Equals(LastExamId));

                return _context.CompletedExams.Any(cl=>cl.UserId== UserId && cl.LastExamId==LastExamId);
            }
        }



    }
}
