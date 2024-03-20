using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.LastExams
{
    public class CreateLastExamCommand : IRequest<ActionResult <LastExamDTO>>
    {
        public int ChapterId { get; set; }
        public LastExamDTO LastExam { get; set; }

        public class CreateLastExamCommandHandler : IRequestHandler<CreateLastExamCommand, ActionResult<LastExamDTO>>
        {
            private readonly Content_ModerationContext _context;

            public CreateLastExamCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }

            public async Task<ActionResult<LastExamDTO>> Handle(CreateLastExamCommand request, CancellationToken cancellationToken)
            {
                var chapter = await _context.Chapters
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.TheoryQuestions)
                        .ThenInclude(tq => tq.AnswerOptions)
                .FirstOrDefaultAsync(c => c.Id == request.ChapterId);
                if (chapter == null)
                {
                    return new BadRequestObjectResult(Message.MSG28);
                }
                var lastExam = new LastExam
                {
                    ChapterId = request.ChapterId,
                    PercentageCompleted = request.LastExam.PercentageCompleted,
                    Name = request.LastExam.Name,
                    Time=request.LastExam.Time,
                };
                chapter.LastExams.Add(lastExam);

                foreach (var qeDTO in request.LastExam.QuestionExams)
                {
                    var qe = new QuestionExam
                    {
                        ContentQuestion=qeDTO.ContentQuestion,
                        Score=qeDTO.Score, 
                        Status=qeDTO.Status,
                        LastExamId=qeDTO.LastExamId
                    };

                    foreach (var answerOptionDTO in qe.AnswerExams)
                    {
                        var newAnswerOption = new AnswerExam
                        {
                           CorrectAnswer=answerOptionDTO.CorrectAnswer, 
                           Exam=answerOptionDTO.Exam,
                           OptionsText=answerOptionDTO.OptionsText,
                        };

                        qe.AnswerExams.Add(newAnswerOption);
                    }

                    lastExam.QuestionExams.Add(qe);
                }

                _context.LastExams.Add(lastExam);
                await _context.SaveChangesAsync();
                var lastexamDTO = new LastExamDTO
                {
                    Id=lastExam.Id,
                    Name=lastExam.Name,
                    ChapterId=lastExam.ChapterId,
                    PercentageCompleted=lastExam.PercentageCompleted,
                    Time=lastExam.Time,

                    QuestionExams = lastExam.QuestionExams.Select(tq => new QuestionExamDTO
                    {
                        Id = tq.Id,
                      ContentQuestion=tq.ContentQuestion,
                      Score=tq.Score,
                      Status=tq.Status,
                      LastExamId=tq.LastExamId,
                        AnswerExams = tq.AnswerExams.Select(ao => new AnswerExamDTO
                        {
                            Id = ao.Id,
                            ExamId=ao.ExamId,
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
