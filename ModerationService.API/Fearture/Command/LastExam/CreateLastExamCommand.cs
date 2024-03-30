using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class CreateLastExamCommand : IRequest<IActionResult>
    {
        public int ChapterId { get; set; }
        public LastExamDTO LastExam { get; set; }

        public class CreateLastExamCommandHandler : IRequestHandler<CreateLastExamCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;

            public CreateLastExamCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(CreateLastExamCommand request, CancellationToken cancellationToken)
            {
                var chapter = await _context.Chapters
                .Include(c => c.LastExams)
                    .ThenInclude(l => l.QuestionExams)
                    .ThenInclude(tq => tq.AnswerExams)
                .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

                // Validate input
                if (string.IsNullOrEmpty(request.LastExam.Name)
                    || request.LastExam.PercentageCompleted == null
                    || request.LastExam.Time == null)
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                // Invalid length
                if (request.LastExam.Name.Length > 256)
                {
                    return new BadRequestObjectResult(Message.MSG27);
                }

                // Invalid number
                if (request.LastExam.PercentageCompleted < 0
                    || request.LastExam.PercentageCompleted > 100
                    || request.LastExam.Time < 0)
                {
                    return new BadRequestObjectResult(Message.MSG26);
                }

                // Check if chapter is exist
                if (chapter == null)
                {
                    return new BadRequestObjectResult(Message.MSG28);
                }

                var lastExam = new LastExam
                {
                    ChapterId = request.ChapterId,
                    PercentageCompleted = request.LastExam.PercentageCompleted,
                    Name = request.LastExam.Name,
                    Time = request.LastExam.Time,
                };
                chapter.LastExams.Add(lastExam);

                foreach (var qeDTO in request.LastExam.QuestionExams)
                {
                    // Validate input
                    if (string.IsNullOrEmpty(qeDTO.ContentQuestion))
                    {
                        return new BadRequestObjectResult(Message.MSG11);
                    }

                    // Invalid length
                    if (qeDTO.ContentQuestion.Length > 256)
                    {
                        return new BadRequestObjectResult(Message.MSG27);
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
                            Id = answerOptionDTO.Id,
                            ExamId= answerOptionDTO.Id
                        };

                        qe.AnswerExams.Add(newAnswerOption);
                    }

                    lastExam.QuestionExams.Add(qe);
                }

                _context.LastExams.Add(lastExam);
                await _context.SaveChangesAsync();

                var lastexamDTO = new LastExamDTO
                {
                    Id = lastExam.Id,
                    Name = lastExam.Name,
                    ChapterId = lastExam.ChapterId,
                    PercentageCompleted = lastExam.PercentageCompleted,
                    Time = lastExam.Time,

                    QuestionExams = lastExam.QuestionExams.Select(tq => new QuestionExamDTO
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

                return new OkObjectResult(lastexamDTO);
            }
        }
    }
}
