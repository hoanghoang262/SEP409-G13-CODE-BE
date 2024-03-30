using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class GetLastExamByIdQuery : IRequest<IActionResult>
    {
        public int LastExamId { get; set; }
    }

    public class GetLastExamByIdQueryHandler : IRequestHandler<GetLastExamByIdQuery, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public GetLastExamByIdQueryHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(GetLastExamByIdQuery request, CancellationToken cancellationToken)
        {
            var lastExam = await _context.LastExams.Include(c=>c.QuestionExams).ThenInclude(c=>c.AnswerExams).FirstOrDefaultAsync(c=>c.Id==request.LastExamId);

            if (lastExam == null)
            {
                return new NotFoundResult();
            }
            var result = new LastExamDTO
            {
                Id = lastExam.Id,
                ChapterId = lastExam.ChapterId,
                PercentageCompleted = lastExam.PercentageCompleted,
                Name = lastExam.Name,
                Time = lastExam.Time,
                QuestionExams = lastExam.QuestionExams.Select(q => new QuestionExamDTO
                {
                    Id=q.Id,
                    ContentQuestion=q.ContentQuestion,
                    LastExamId=q.LastExamId,
                    Score=q.Score,
                    Status=q.Status,
                    AnswerExams= q.AnswerExams.Select(q=> new AnswerExamDTO
                    {
                        ExamId=q.ExamId,
                        CorrectAnswer=q.CorrectAnswer,
                        Id=q.Id,
                        OptionsText=q.OptionsText

                    }).ToList(),
                }).ToList(),
            };

            return new OkObjectResult(result);
        }
    }
}
