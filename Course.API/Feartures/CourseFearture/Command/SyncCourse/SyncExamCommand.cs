using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncExamCommand : IRequest<IActionResult>, IMapFrom<ExamEvent>
    {
        public int Id { get; set; }
        public string? ContentQuestion { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public bool? Status { get; set; }
        public int LastExamId { get; set; }

        public class SyncExamCommandHandler : IRequestHandler<SyncExamCommand, IActionResult>
        {
            private readonly CourseContext _context;
            public SyncExamCommandHandler(CourseContext context)
            {
                _context = context;

            }
            public async Task<IActionResult> Handle(SyncExamCommand request, CancellationToken cancellationToken)
            {
                var ex = _context.Exams.FirstOrDefault(x => x.Id.Equals(request.Id));
                if (ex == null)
                {
                    var newEx = new Exam
                    {
                        Id = request.Id,
                       
                        ContentQuestion=request.ContentQuestion,
                        Score=request.Score,
                        Status=request.Status,
                        Time=request.Time

                    };

                    _context.Exams.Add(newEx);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {

                   
                    ex.ContentQuestion = request.ContentQuestion;
                    ex.Score = request.Score;
                    ex.Status = request.Status;
                    ex.Time = request.Time;
                    ex.LastExamId = request.LastExamId;




                    await _context.SaveChangesAsync(cancellationToken);
                }
                return new OkObjectResult("done");
            }
        }
    }
}
