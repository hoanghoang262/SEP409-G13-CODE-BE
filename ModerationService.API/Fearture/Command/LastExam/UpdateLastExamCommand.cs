using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class UpdateLastExamCommand : IRequest<IActionResult>
    {
        public int LastExamId { get; set; }
        public LastExamDTO LastExam { get; set; }
    }

    public class UpdateLastExamCommandHandler : IRequestHandler<UpdateLastExamCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public UpdateLastExamCommandHandler(Content_ModerationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(UpdateLastExamCommand request, CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.LastExam.Name)
                || request.LastExam.PercentageCompleted == null
                || request.LastExam.Time == null)
            {
                return new BadRequestObjectResult(Message.MSG11);
            }
            if (request.LastExam.Name.Length > 256)
            {
                return new BadRequestObjectResult(Message.MSG27);
            }
            if (request.LastExam.PercentageCompleted < 0
                || request.LastExam.PercentageCompleted > 100
                || request.LastExam.Time < 0)
            {
                return new BadRequestObjectResult(Message.MSG26);
            }

            var existingLastExam = await _context.LastExams
             .Include(l => l.QuestionExams)
                 .ThenInclude(tq => tq.AnswerExams)
             .FirstOrDefaultAsync(l => l.Id == request.LastExamId);

            // Check if lesson exists
            if (existingLastExam == null)
            {
                return new BadRequestObjectResult(Message.MSG32);
            }

            existingLastExam.Time = request.LastExam.Time;
            existingLastExam.Name = request.LastExam.Name;
            existingLastExam.ChapterId = request.LastExam.ChapterId;
            existingLastExam.PercentageCompleted = request.LastExam.PercentageCompleted;



            if (existingLastExam != null)
            {
                foreach (var questionExam in existingLastExam.QuestionExams)
                {
                    _context.AnswerExams.RemoveRange(questionExam.AnswerExams);
                }

                _context.QuestionExams.RemoveRange(existingLastExam.QuestionExams);
          

                await _context.SaveChangesAsync();
            }


            foreach (var qeDTO in request.LastExam.QuestionExams)
            {
               
                if (string.IsNullOrEmpty(qeDTO.ContentQuestion))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                // Invalid length
                if (qeDTO.ContentQuestion.Length > 256)
                {
                    return new BadRequestObjectResult(Message.MSG27);
                }

                // Invalid number
                if (qeDTO.Score < 0)
                {
                    return new BadRequestObjectResult(Message.MSG26);
                }

                var qe = new QuestionExam
                {
                    ContentQuestion = qeDTO.ContentQuestion,
                    Score = qeDTO.Score,
                    Status = qeDTO.Status,
                    LastExamId = qeDTO.LastExamId
                };

                foreach (var answerOptionDTO in qeDTO.AnswerExams)
                {
                    // Validate input
                    if (answerOptionDTO.CorrectAnswer == null
                        && string.IsNullOrEmpty(answerOptionDTO.OptionsText))
                    {
                        return new BadRequestObjectResult(Message.MSG11);
                    }

                    // Invalid length
                    if (!string.IsNullOrEmpty(answerOptionDTO.OptionsText)
                        && answerOptionDTO.OptionsText.Length > 256)
                    {
                        return new BadRequestObjectResult(Message.MSG27);
                    }

                    var newAnswerOption = new AnswerExam
                    {
                        CorrectAnswer = answerOptionDTO.CorrectAnswer,
                        OptionsText = answerOptionDTO.OptionsText,
                        ExamId= answerOptionDTO.ExamId
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
