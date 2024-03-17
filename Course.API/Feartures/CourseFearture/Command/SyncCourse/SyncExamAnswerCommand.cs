using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncExamAnswerCommand : IRequest<IActionResult>, IMapFrom<AnswerExamEvent>
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }

        public class SyncExamAnswerCommandHandler : IRequestHandler<SyncExamAnswerCommand, IActionResult>
        {
            private readonly CourseContext _context;
            public SyncExamAnswerCommandHandler(CourseContext context)
            {
                _context = context;

            }
            public async  Task<IActionResult> Handle(SyncExamAnswerCommand request, CancellationToken cancellationToken)
            {
                var exAns = _context.AnswerExams.FirstOrDefault(x => x.Id.Equals(request.Id));
                if (exAns == null)
                {
                    var newExAns = new AnswerExam
                    {
                        Id = request.Id,
                        CorrectAnswer=request.CorrectAnswer,
                        ExamId=request.ExamId,
                        OptionsText=request.OptionsText

                    };

                    _context.AnswerExams.Add(newExAns);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {

                    exAns.CorrectAnswer = request.CorrectAnswer;
                    exAns.ExamId = request.ExamId;
                    exAns.OptionsText = request.OptionsText;
                   
                    


                    await _context.SaveChangesAsync(cancellationToken);
                }
                return new OkObjectResult("done");
            }
        }
    }
}
