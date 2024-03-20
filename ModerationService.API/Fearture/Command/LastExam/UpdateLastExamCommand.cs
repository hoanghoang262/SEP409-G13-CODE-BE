using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class UpdateLastExamCommand : IRequest<ActionResult<LastExamDTO>>
    {
        public int LastExamId { get; set; }
        public LastExamDTO LastExam { get; set; }
    }

    public class UpdateLastExamCommandHandler : IRequestHandler<UpdateLastExamCommand, ActionResult<LastExamDTO>>
    {
        private readonly Content_ModerationContext _context;

        public UpdateLastExamCommandHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<LastExamDTO>> Handle(UpdateLastExamCommand request, CancellationToken cancellationToken)
        {
            var existingLastExam = await _context.LastExams
             .Include(l => l.QuestionExams)
                 .ThenInclude(tq => tq.AnswerExams)
             .FirstOrDefaultAsync(l => l.Id == request.LastExamId);

            // Check if lesson exists
            if (existingLastExam == null)
            {
                return new BadRequestObjectResult(Message.MSG29);
            }

           
            existingLastExam.Time = request.LastExam.Time;
            existingLastExam.Name = request.LastExam.Name;
            existingLastExam.ChapterId = request.LastExam.ChapterId;
            existingLastExam.PercentageCompleted = request.LastExam.PercentageCompleted;



            // Clear existing questions and options
            existingLastExam.QuestionExams.Clear();

            foreach (var qeDTO in request.LastExam.QuestionExams)
            {
                var qe = new QuestionExam
                {
                    ContentQuestion = qeDTO.ContentQuestion,
                    Score = qeDTO.Score,
                    Status = qeDTO.Status,
                    LastExamId = qeDTO.LastExamId
                };

                foreach (var answerOptionDTO in qe.AnswerExams)
                {
                    var newAnswerOption = new AnswerExam
                    {
                        CorrectAnswer = answerOptionDTO.CorrectAnswer,
                        Exam = answerOptionDTO.Exam,
                        OptionsText = answerOptionDTO.OptionsText,
                    };

                    qe.AnswerExams.Add(newAnswerOption);
                }

                existingLastExam.QuestionExams.Add(qe);
            }

            await _context.SaveChangesAsync();

            var lastexamDTO = new LastExamDTO
            {
                Id = existingLastExam.Id,
                Name = existingLastExam.Name,
                ChapterId = existingLastExam.ChapterId,
                PercentageCompleted = existingLastExam.PercentageCompleted,
                Time = existingLastExam.Time,

                QuestionExams = existingLastExam.QuestionExams.Select(tq => new QuestionExamDTO
                {
                    Id = tq.Id,
                    ContentQuestion = tq.ContentQuestion,
                    Score = tq.Score,
                    Status = tq.Status,
                    LastExamId = tq.LastExamId,
                    AnswerExams = tq.AnswerExams.Select(ao => new AnswerExamDTO
                    {
                        Id = ao.Id,
                        ExamId = ao.ExamId,
                        OptionsText = ao.OptionsText,
                        CorrectAnswer = ao.CorrectAnswer
                    }).ToList()
                }).ToList()
            };

            await _context.SaveChangesAsync();

            return new OkObjectResult(lastexamDTO);
        }
    }
}
